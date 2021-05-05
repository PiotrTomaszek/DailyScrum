using DailyScrum.Areas.Identity.Data;
using DailyScrum.Data;
using DailyScrum.Models.Database;
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
        private static HashSet<string> _connectedUsers = new HashSet<string>();
        private static Dictionary<string, List<ApplicationUser>> _connectedTeams = new Dictionary<string, List<ApplicationUser>>();
        //private readonly static HashSet<ApplicationUser> _connectedUsers = new HashSet<ApplicationUser>();
        //private readonly static Dictionary<string, List<ApplicationUser>> _connecte

        private readonly DailyScrumContext _dbContext;

        private string SignalRIdentityName => Context.User.Identity.Name;

        public DailyHub(DailyScrumContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<Task> OnConnectedAsync()
        {
            var teamToCreate = _connectedUsers.Add(Context.ConnectionId);

            var user = _dbContext.Users.Include(a => a.TeamMember)
                .Where(x => x.UserName == SignalRIdentityName)
                .FirstOrDefault();

            if (_connectedTeams.ContainsKey(user.TeamMember.Name) == false)
            {
                _connectedTeams.Add(user.TeamMember.Name, new List<ApplicationUser> { user });
                await Groups.AddToGroupAsync(Context.ConnectionId, user.TeamMember.Name);
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

            _connectedTeams.TryGetValue(user.TeamMember.Name, out List<ApplicationUser> teamMembers);

            foreach (var item in teamMembers)
            {
                if (item.UserName != SignalRIdentityName)
                {

                }

                await Clients.Caller.SendAsync("UserConnected", $"{item.FirstName} {item.LastName}", item.Email, item.Id, item.PhotoPath);
            }

            await Clients.OthersInGroup(user.TeamMember.Name).SendAsync("UserConnected", $"{user.FirstName} {user.LastName}", user.Email, user.Id, user.PhotoPath);

            //await Clients.Group(user.TeamMember.Name).SendAsync("UserConnected", $"{user.FirstName} {user.LastName}", user.Email, user.Id, user.PhotoPath);

            //await Clients.All.SendAsync("TestMethod", SignalRIdentityName, user.TeamMember.Name);
            //await Clients.Group(user.TeamMember.Name).SendAsync("TestMethod", SignalRIdentityName, "TUSTE");
            //await Clients.OthersInGroup(user.TeamMember.Name).SendAsync("TestMethod", SignalRIdentityName, "SUPERTUSTE");

            return base.OnConnectedAsync();

            //var user = _dbContext.Users.Include(a => a.TeamMember)
            //    .Where(x => x.UserName == SignalRIdentityName)
            //    .FirstOrDefault();

            //if (user != null && !(_connectedUsers.Contains(user)))
            //{
            //    _connectedUsers.Add(user);
            //}

            //if (user.TeamMember != null)
            //{
            //    var team = _dbContext.Teams.Find(user.TeamMember.TeamId);
            //    await AddToGroup(team.Name);
            //}

            //foreach (var item in _connectedUsers)
            //{
            //    if (item.UserName == SignalRIdentityName)
            //    {
            //        continue;
            //    }
            //    await Clients.Caller.SendAsync("UserConnected", $"{item.FirstName} {item.LastName}", item.Email, item.Id, item.PhotoPath);
            //}

            //await Clients.Group(user.TeamMember.Name).SendAsync("UserConnected", $"{user.FirstName} {user.LastName}", user.Email, user.Id, user.PhotoPath);


        }

        public async override Task<Task> OnDisconnectedAsync(Exception exception)
        {
            var user = _dbContext.Users.Include(a => a.TeamMember)
                .Where(x => x.UserName == SignalRIdentityName)
                .FirstOrDefault();

            _connectedUsers.Remove(Context.ConnectionId);

            _connectedTeams.TryGetValue(user.TeamMember.Name, out List<ApplicationUser> members);

            if (members != null)
            {
                members.Remove(user);
            }

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

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("TestMethod", this.Context.User.Identity.Name, message);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("TestMethod", this.Context.User.Identity.Name, groupName);
        }

        //public IEnumerable<ApplicationUser> GetUsers()
        //{
        //    return _connectedUsers.ToList();
        //}
    }
}
