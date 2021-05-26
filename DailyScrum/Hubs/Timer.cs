using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace DailyScrum.Hubs
{
    [Authorize]
    public partial class DailyHub : Hub
    {
        public async Task DisplayStartTime()
        {
            await Clients.Group(DbUser.TeamMember.Name).SendAsync("DisplayStartTime", TeamModel.DailyMeeting?.Date);
        }

        public async Task DisplayTimer()
        {
            //na szutke czas
            var endTime = TeamModel.DailyMeeting?.Date.AddMinutes(15).Ticks;
            var nowTime = DateTime.UtcNow.Ticks;

            var time = (endTime - nowTime) / 10000000;

            await Clients.Caller.SendAsync("DisplayTimer", TeamModel.IsDailyStarted, time);
        }

        public async Task SetUpTimer()
        {
            var endTime = TeamModel.DailyMeeting?.Date.AddMinutes(15).Ticks;
            var nowTime = DateTime.UtcNow.Ticks;

            var time = (endTime - nowTime) / 10000000;

            await Clients.Group(DbUser.TeamMember.Name).SendAsync("DisplayTimer", TeamModel.IsDailyStarted, time);
        }

        public async Task DisableTimer()
        {
            await Clients.Group(DbUser.TeamMember.Name).SendAsync("DisplayTimer", TeamModel.IsDailyStarted, -1);
        }

        public async Task CheckIfDailyHasEnded()
        {
            var test = TeamModel.MeetingStartingTime;

            if (test.AddMinutes(15) < DateTime.Now)
            {
                if (TeamModel?.DailyMeeting != null && TeamModel.IsDailyStarted == true)
                {
                    await EndDailyMeeting();
                }
            }
        }
    }
}
