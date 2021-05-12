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
        private string SignalRIdentityName => Context.User.Identity.Name;

        private static HashSet<string> _connectedUsers = new HashSet<string>();
        private static Dictionary<string, List<ApplicationUser>> _connectedTeams = new Dictionary<string, List<ApplicationUser>>();
        private static Dictionary<string, TeamViewModel> _connectedTeamsInfo = new Dictionary<string, TeamViewModel>();
        //private static Dictionary<string, List<MessageViewModel>> _teamMessages = new Dictionary<string, List<MessageViewModel>>();

        private readonly DailyScrumContext _dbContext;

        public DailyHub(DailyScrumContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ShowTeamName(string teamName)
        {
            await Clients.Caller.SendAsync("DisplayTeamName", teamName);
        }

        public async Task GenerateConnectedUsers(string teamName)
        {
            var test = _connectedTeamsInfo.TryGetValue(teamName, out TeamViewModel model);
            await Clients.Caller.SendAsync("GenerateAllUsers", model.ConnectedUsersCount, model.TeamMemberCount);
        }

        public async Task UpdateUserList(string teamName)
        {
            var test = _connectedTeamsInfo.TryGetValue(teamName, out TeamViewModel model);
            await Clients.OthersInGroup(teamName).SendAsync("UpdateUserList", model.ConnectedUsersCount, model.TeamMemberCount);
        }

        //public async List<string> GetAllTeamMembers()
        //{
        //    return _connectedTeams.TryGetValue();
        //}

        public override async Task<Task> OnConnectedAsync()
        {
            var teamToCreate = _connectedUsers.Add(Context.ConnectionId);

            var user = _dbContext.Users.Include(a => a.TeamMember)
                .Where(x => x.UserName == SignalRIdentityName)
                .FirstOrDefault();

            if (_connectedTeams.ContainsKey(user.TeamMember.Name) == false)
            {
                var teamMates = await _dbContext.Users
                    .Include(x => x.TeamMember)
                    .Where(a => a.TeamMember.Name.Equals(user.TeamMember.Name)).ToListAsync();

                _connectedTeams.Add(user.TeamMember.Name, new List<ApplicationUser>(teamMates));

                var number = _dbContext.Users
                     .Include(x => x.TeamMember)
                     .Where(a => a.TeamMember.Name.Equals(user.TeamMember.Name))
                     .Count();

                var mod = new TeamViewModel
                {
                    ConnectedUsersCount = 1,
                    TeamMemberCount = number
                };

                _connectedTeamsInfo.Add(user.TeamMember.Name, mod);

                await Groups.AddToGroupAsync(Context.ConnectionId, user.TeamMember.Name);
            }
            else
            {
                _connectedTeamsInfo.TryGetValue(user.TeamMember.Name, out TeamViewModel model);
                model.ConnectedUsersCount += 1;
                await UpdateUserList(user.TeamMember.Name);
            }



            if (_connectedTeams.TryGetValue(user.TeamMember.Name, out List<ApplicationUser> members))
            {
                if (members.Where(u => u.Id == user.Id).FirstOrDefault() == null)
                {
                    members.Add(user);
                    await Groups.AddToGroupAsync(Context.ConnectionId, user.TeamMember.Name);


                    //await Groups.AddToGroupAsync(Context.ConnectionId, user.TeamMember.Name);
                }
            }

            //test
            await ShowTeamName(user.TeamMember.Name);
            await GenerateConnectedUsers(user.TeamMember.Name);




            _connectedTeams.TryGetValue(user.TeamMember.Name, out List<ApplicationUser> teamMembers);

            foreach (var item in teamMembers)
            {
                await Clients.Caller.SendAsync("UserConnected", $"{item.FirstName} {item.LastName}", item.Email, item.Id, item.PhotoPath);
            }

            await Clients.OthersInGroup(user.TeamMember.Name).SendAsync("UserConnected", $"{user.FirstName} {user.LastName}", user.Email, user.Id, user.PhotoPath);

            //await Clients.Group(user.TeamMember.Name).SendAsync("UserConnected", $"{user.FirstName} {user.LastName}", user.Email, user.Id, user.PhotoPath);

            await Clients.Group(user.TeamMember.Name).SendAsync("NotifyJoinedUser", $"{user.LastName} {user.FirstName}");

            //await Clients.All.SendAsync("TestMethod", SignalRIdentityName, user.TeamMember.Name);
            //await Clients.Group(user.TeamMember.Name).SendAsync("TestMethod", SignalRIdentityName, "TUSTE");
            //await Clients.OthersInGroup(user.TeamMember.Name).SendAsync("TestMethod", SignalRIdentityName, "SUPERTUSTE");

            return base.OnConnectedAsync();
        }


        public async override Task<Task> OnDisconnectedAsync(Exception exception)
        {
            var user = _dbContext.Users.Include(a => a.TeamMember)
                .Where(x => x.UserName == SignalRIdentityName)
                .FirstOrDefault();

            _connectedUsers.Remove(Context.ConnectionId);

            _connectedTeams.TryGetValue(user.TeamMember.Name, out List<ApplicationUser> members);

            _connectedTeamsInfo.TryGetValue(user.TeamMember.Name, out TeamViewModel model);

            model.ConnectedUsersCount -= 1;
            await UpdateUserList(user.TeamMember.Name);

            if (model.ConnectedUsersCount == 0)
            {
                _connectedTeamsInfo.Remove(user.TeamMember.Name);
            }

            if (members != null)
            {
                members.Remove(user);
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.TeamMember.Name);

            await Clients.Group(user.TeamMember.Name).SendAsync("UserDisconnected", user.Id);

            //var user = _dbContext.Users.Include(a => a.TeamMember)
            //    .Where(x => x.UserName == SignalRIdentityName)
            //    .FirstOrDefault();

            //if (user != null)
            //{
            //    _connectedUsers.Remove(user);
            //}

            //await Clients.Group(user.TeamMember.Name).SendAsync("UserDisconnected", user.Id);

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message)
        {
            await Clients.Group("DEV1").SendAsync("TestMethod", this.Context.User.Identity.Name, message);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("TestMethod", this.Context.User.Identity.Name, groupName);
        }

    }
}
