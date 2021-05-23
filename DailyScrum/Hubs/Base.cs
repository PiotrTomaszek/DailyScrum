using DailyScrum.Areas.Identity.Data;
using DailyScrum.Data;
using DailyScrum.Repository;
using DailyScrum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
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
        private readonly ITeamRepository _teamRepository;
        private readonly IMessageRepository _messageRepository;

        private string SignalRIdentityName => Context.User.Identity.Name;

        private ApplicationUser DbUser => _connectedUsers
            .Where(x => x.Key == this.Context.ConnectionId)
            .FirstOrDefault().Value;

        public DailyHub(DailyScrumContext dbContext,
            IProblemRepository problemRepository,
            IUserRepository userRepository,
            IDailyMeetingRepository dailyRepository,
            IPostRepository postRepository,
            ITeamRepository teamRepository,
            IMessageRepository messageRepository)
        {
            _dbContext = dbContext;
            _problemRepository = problemRepository;
            _userRepository = userRepository;
            _dailyRepository = dailyRepository;
            _postRepository = postRepository;
            _teamRepository = teamRepository;
            _messageRepository = messageRepository;
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

                await GetMessages();
                await GetAllPosts();
                await EnableSubmitButton();


                await AddDailyOptions();

                await GetDailyOptions();

                await CheckIfDailyHasEnded();
                await DisplayTimer();

                //await AddNotificationSystemToUser();

                await DisplayNotifications();
            }

            return base.OnConnectedAsync();
        }

        public async override Task<Task> OnDisconnectedAsync(Exception exception)
        {
            if (_userRepository.CheckIfHasTeam(SignalRIdentityName))
            {
                // wyrzuca nulla jak nie ma zespolu a sie wyloguje
                if (DbUser.TeamMember?.Name != null)
                {
                    await HandleTeamMemberNumber(-1);

                    await SetUserStatus(false);
                }
            }

            _connectedUsers.Remove(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
