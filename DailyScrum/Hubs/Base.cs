using DailyScrum.Areas.Identity.Data;
using DailyScrum.Data;
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

        private string SignalRIdentityName => Context.User.Identity.Name;
        private ApplicationUser DbUser => _connectedUsers
            .Where(x => x.Key == this.Context.ConnectionId)
            .FirstOrDefault().Value;
        private ApplicationUser GetUser()
        {
            var user = _dbContext.Users
                .Include(a => a.TeamMember)
                .Include(ro => ro.TeamRole)
                .Where(x => x.UserName == SignalRIdentityName)
                .FirstOrDefault();

            return user;
        }


        public DailyHub(DailyScrumContext dbContext)
        {
            _dbContext = dbContext;
        }


        public override async Task<Task> OnConnectedAsync()
        {
            //powiazanie connId z userem ->
            //tutaj jest bug ktory incrementuje ilosc zalogowanych osob
            //jezeli zalogujesz sie jeszcze raz na zalogowane konto
            _connectedUsers.Add(Context.ConnectionId, GetUser());

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

            //await TimeStuff();
            //SetUpTimer(new TimeSpan(11, 23, 00));

            return base.OnConnectedAsync();
        }

        public async override Task<Task> OnDisconnectedAsync(Exception exception)
        {
            await HandleTeamMemberNumber(-1);

            await SetUserStatus(false);

            _connectedUsers.Remove(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
