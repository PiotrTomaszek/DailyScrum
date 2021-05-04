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
        private readonly static List<ApplicationUser> _connectedUsers = new List<ApplicationUser>();

        private readonly DailyScrumContext _dbContext;

        public DailyHub(DailyScrumContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<Task> OnConnectedAsync()
        {
            var test = Context.User.Identity.Name;

            var user = _dbContext.Users.Include(a => a.TeamMember)
                .Where(x => x.UserName == test)
                .FirstOrDefault();

            if (user != null)
            {
                _connectedUsers.Add(user);
            }

            if (user.TeamMember != null)
            {
                var team = _dbContext.Teams.Find(user.TeamMember.TeamId);
                await AddToGroup(team.Name);
            }

            await Clients.Group(user.TeamMember.Name).SendAsync("UserConnected", $"{user.FirstName} {user.LastName}", user.Email,user.Id, user.PhotoPath);

            return base.OnConnectedAsync();
        }

        public async override Task<Task> OnDisconnectedAsync(Exception exception)
        {
            var test = Context.User.Identity.Name;

            var user = _dbContext.Users.Include(a => a.TeamMember)
                .Where(x => x.UserName == test)
                .FirstOrDefault();

            if (user != null)
            {
                _connectedUsers.Remove(user);
            }

            await Clients.Group(user.TeamMember.Name).SendAsync("UserDisconnected", user.Id);

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

        public IEnumerable<ApplicationUser> GetUsers()
        {
            return _connectedUsers.ToList();
        }
    }
}
