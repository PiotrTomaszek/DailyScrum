using DailyScrum.Data;
using DailyScrum.Repository;
using DailyScrum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
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




        // jest miodzio
        public async Task/*<Task>*/ EndDailyMeetingTimer(TimeSpan timeSpan)
        {
            var dummyTask = Task.Run(() =>
            {
                Thread.Sleep(timeSpan);
            });


            Task.WaitAll(dummyTask);

            await EndDailyMeeting();

            //return dummyTask;
        }
    }
}
