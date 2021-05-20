using DailyScrum.Areas.Identity.Data;
using DailyScrum.Data;
using DailyScrum.Repository;
using DailyScrum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Hubs
{
    [Authorize]
    public partial class DailyHub : Hub
    {
        private static Dictionary<string, ApplicationUser> _connectedUsers = new Dictionary<string, ApplicationUser>();
        private static Dictionary<string, TeamViewModel> _connectedTeams = new Dictionary<string, TeamViewModel>();

        private readonly DailyScrumContext _dbContext;

        private readonly IProblemRepository _problemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDailyMeetingRepository _dailyRepository;
        private readonly IPostRepository _postRepository;


        private string SignalRIdentityName => Context.User.Identity.Name;

        private ApplicationUser DbUser => _connectedUsers
            .Where(x => x.Key == this.Context.ConnectionId)
            .FirstOrDefault().Value;

        public DailyHub(DailyScrumContext dbContext,
            IProblemRepository problemRepository,
            IUserRepository userRepository,
            IDailyMeetingRepository dailyRepository,
            IPostRepository postRepository)
        {
            _dbContext = dbContext;
            _problemRepository = problemRepository;
            _userRepository = userRepository;
            _dailyRepository = dailyRepository;
            _postRepository = postRepository;
        }


        public override async Task<Task> OnConnectedAsync()
        {
            //powiazanie connId z userem ->
            //tutaj jest bug ktory incrementuje ilosc zalogowanych osob
            //jezeli zalogujesz sie jeszcze raz na zalogowane konto
            _connectedUsers.Add(Context.ConnectionId, _userRepository.GetUserByUserName(SignalRIdentityName));

            // test notyfikacji

            //_usersNotifications.Add(DbUser.UserName, new NotificationViewModel());


            // tutaj zrobie sobie sprawdzenie czy ma zespol bo w przeciwnym wypadku twywali signala
            if (_userRepository.CheckIfHasTeam(SignalRIdentityName))
            {
                //sprawdzenie czy istieje ten zespol
                await HandleNewTeam();

                await Groups.AddToGroupAsync(Context.ConnectionId, DbUser.TeamMember.Name);


                await ShowTeamName(DbUser.TeamMember.Name);
                await HandleTeamMemberNumber(1);
                await GenerateConnectedUsers(DbUser.TeamMember.Name);


                await GenerateUserList();

                await GetAllUsersStatus();
                await SetUserStatus(true);

                await GetAllMessages();
                await GetAllPosts();
                await EnableSubmitButton();


                await AddDailyOptions();

                await GetDailyOptions();

                //await EnableScrumMasterProblemBar();

                //await TimeStuff();
                //SetUpTimer(new TimeSpan(11, 23, 00));

            }

            return base.OnConnectedAsync();
        }

        // chyba bedzie do wyrzucenia bo wszyskniecie w widoku jest lepsze
        //public async Task EnableScrumMasterProblemBar()
        //{
        //    if (DbUser.TeamRole?.RoleId == 1)
        //    {
        //        await Clients.Caller.SendAsync("GenScrumMasterProblems");
        //    }
        //}

        public async override Task<Task> OnDisconnectedAsync(Exception exception)
        {
            if (!_userRepository.CheckIfHasTeam(SignalRIdentityName))
            {
                // wyrzuca nulla jak nie ma zespolu a sie wyloguje
                await HandleTeamMemberNumber(-1);

                await SetUserStatus(false);

            }

            //test
            //_usersNotifications.Remove(DbUser.UserName);

            _connectedUsers.Remove(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
