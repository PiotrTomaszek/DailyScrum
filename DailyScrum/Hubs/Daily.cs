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
        public async Task SendPost(string yesterday, string today, string problem)
        {
            var time = DateTime.UtcNow.ToShortTimeString();

            await Clients.Group(DbUser.TeamMember.Name).SendAsync("SendDailyPost",UserFullName, yesterday, today, problem, time, DbUser.Id);
        }
    }
}
