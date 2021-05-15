using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DailyScrum.Hubs
{
    [Authorize]
    public partial class DailyHub : Hub
    {
        public async Task TimeStuff()
        {
            _connectedTeams.TryGetValue(DbUser.TeamMember.Name, out var model);

            await Clients.Caller.SendAsync("DisplayTime", model.MeetingStartingTime.ToString());
        }

        // dzioa ale do ogarniecia
        //private System.Threading.Timer timer;
        //private void SetUpTimer(TimeSpan alertTime)
        //{
        //    var current = DateTime.Now;
        //    var timeToGo = alertTime - current.TimeOfDay;

        //    if (timeToGo < TimeSpan.Zero)
        //    {
        //        return;
        //    }

        //    this.timer = new System.Threading.Timer(x =>
        //    {
        //        this.SomeMethodRunsAt1600();
        //    }, null, timeToGo, Timeout.InfiniteTimeSpan);
        //}
        //private void SomeMethodRunsAt1600()
        //{
        //    var a = "a";
        //}
    }
}
