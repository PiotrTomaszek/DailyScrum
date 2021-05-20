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
            await Clients.Caller.SendAsync("DisplayTime", TeamModel.MeetingStartingTime.ToString());
        }

        // dzioa ale do ogarniecia

        private void SetUpTimer(TimeSpan alertTime, string teamName)
        {
            var current = DateTime.Now;
            var timeToGo = alertTime - current.TimeOfDay;

            if (timeToGo < TimeSpan.Zero)
            {
                return;
            }

            TeamModel.Timer = new System.Threading.Timer(x =>
           {
               EndDailyMeetingByTime(teamName);
           }, null, timeToGo, Timeout.InfiniteTimeSpan);
        }
        private async Task EndDailyMeetingByTime(string teamName)
        {
            _connectedTeams.TryGetValue(teamName, out var teamModel);

            teamModel.IsDailyStarted = false;
            if (!teamModel.IsDailyStarted)
            {
                _dailyRepository.EndDailyMeeting(teamModel.DailyMeeting.DailyMeetingId);

                foreach (var item in _userRepository.GetAllTeamMebers(DbUser.TeamMember.Name))
                {
                    var conn = _connectedUsers.Where(x => x.Value.Id == item.Id).FirstOrDefault().Key;

                    await SetEnabledOptions();

                    await Clients.Client(conn).SendAsync("EndDaily");
                }
            }
        }
    }
}
