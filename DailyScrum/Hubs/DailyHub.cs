using DailyScrum.Areas.Identity.Data;
using DailyScrum.Data;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Hubs
{
    public class DailyHub : Hub
    {
        private readonly static List<ApplicationUser> _connectedUsers = new List<ApplicationUser>();
        //private readonly static List<Meeting> _meeting;

        private readonly DailyScrumContext _dbContext;

        public DailyHub(DailyScrumContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override Task OnConnectedAsync()
        {
            var test = Context.User.Identity.Name;

            var user = _dbContext.Users
                .Where(x => x.UserName == test)
                .FirstOrDefault();

            if (user != null)
            {
                _connectedUsers.Add(user);
            }
            //var team = _dbContext.Teams.Find(user.TeamMember.TeamId);

            return base.OnConnectedAsync();
        }



        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("TestMethod", this.Context.User.Identity.Name, message);
        }
    }
}
