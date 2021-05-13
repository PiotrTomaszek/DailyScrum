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
    public class DailyHub : Hub
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
            var user = _dbContext.Users.Include(a => a.TeamMember)
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
            //powiazanie connId z userem
            _connectedUsers.Add(Context.ConnectionId, GetUser());

            //sprawdzenie czy istieje ten zespol
            await HandleNewTeam();

            await Groups.AddToGroupAsync(Context.ConnectionId, DbUser.TeamMember.Name);

            // ustaw status jako online


            await ShowTeamName(DbUser.TeamMember.Name);
            await HandleTeamMemberNumber(1);
            await GenerateConnectedUsers(DbUser.TeamMember.Name);


            await GenerateUserList();


            //TU JEST PROBLEM

            await GetAllUsersStatus();
            await SetUserStatus(true);

            return base.OnConnectedAsync();
        }





        public async override Task<Task> OnDisconnectedAsync(Exception exception)
        {
            await HandleTeamMemberNumber(-1);

            await SetUserStatus(false);

            _connectedUsers.Remove(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }


        private async Task GetAllUsersStatus()
        {
            _connectedTeams.TryGetValue(DbUser.TeamMember.Name, out var model);

            int index = 0;
            foreach (var item in model.UsersOnline)
            {
                var id = model.UsersList[index].Id;
                var isOnline = item;

                await Clients.Caller.SendAsync("SetUserStatus", id, item);

                index++;
            }
        }

        private async Task SetUserStatus(bool isOnline)
        {
            var isOK = _connectedUsers.TryGetValue(Context.ConnectionId, out var connUser);

            if (isOK)
            {
                _connectedTeams.TryGetValue(DbUser.TeamMember.Name, out var model);

                var index = model.UsersList.IndexOf(model.UsersList.Where(x => x.Email.Equals(connUser.Email)).FirstOrDefault());
                //var onlineUser = ;

                if (isOnline)
                {
                    model.UsersOnline[index] = true;
                }
                else
                {
                    model.UsersOnline[index] = false;
                }
            }

            await Clients.Group(DbUser.TeamMember.Name).SendAsync("SetUserStatus", DbUser.Id, isOnline);
        }

        private async Task GenerateUserList()
        {
            _connectedTeams.TryGetValue(DbUser.TeamMember.Name, out var model);

            foreach (var item in model.UsersList)
            {
                var photo = item.PhotoPath == null ? "no-avatar.jpg" : item.PhotoPath;

                await Clients.Caller.SendAsync("UserConnected", $"{item.FirstName} {item.LastName}", item.Email, item.Id, photo);
            }
        }




        public async Task UpdateUserList(string teamName)
        {
            _connectedTeams.TryGetValue(teamName, out TeamViewModel model);
            //await Clients.OthersInGroup(teamName).SendAsync("UpdateUserList", model.ConnectedUsersCount, model.TeamMemberCount);
            await Clients.Group(teamName).SendAsync("UpdateUserList", model.ConnectedUsersCount, model.TeamMemberCount);
        }





        public async Task SendMessage(string message)
        {
            await Clients.Group("DEV1").SendAsync("TestMethod", this.Context.User.Identity.Name, message);
        }




        //ok
        public async Task ShowTeamName(string teamName)
        {
            await Clients.Caller.SendAsync("DisplayTeamName", teamName);
        }

        private async Task HandleNewTeam()
        {
            if (!_connectedTeams.ContainsKey(DbUser.TeamMember.Name))
            {
                var teamMates = await _dbContext.Users
                    .Include(x => x.TeamMember)
                    .Where(a => a.TeamMember.Name.Equals(DbUser.TeamMember.Name)).OrderBy(order => order.LastName).ToListAsync();

                var teamModel = new TeamViewModel
                {
                    UsersList = teamMates,
                    TeamMemberCount = teamMates.Count(),
                    UsersOnline = Enumerable.Repeat(false, teamMates.Count()).ToList()
                };

                _connectedTeams.Add(DbUser.TeamMember.Name, teamModel);
            }
        }

        public async Task GenerateConnectedUsers(string teamName)
        {
            _connectedTeams.TryGetValue(teamName, out TeamViewModel model);
            await Clients.Caller.SendAsync("GenerateUserCounter", model.ConnectedUsersCount, model.TeamMemberCount);
        }

        private async Task HandleTeamMemberNumber(int operation)
        {
            var team = _connectedTeams.TryGetValue(DbUser.TeamMember.Name, out var teamModel);

            if (team)
            {
                teamModel.ConnectedUsersCount += operation;
            }

            await UpdateUserList(DbUser.TeamMember.Name);
        }
    }
}
