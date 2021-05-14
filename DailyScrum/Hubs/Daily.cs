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
        public async Task SendPost(string cos)
        {
            await Clients.Group(DbUser.TeamMember.Name).SendAsync("SendDailyPost", cos);
        }
    }
}
