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
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DailyScrum.Hubs
{
    [Authorize]
    public partial class DailyHub : Hub
    {
        //private static Dictionary<string, Task> _timers = new Dictionary<string, Task>();

        public async Task DisplayStartTime()
        {
            await Clients.Group(DbUser.TeamMember.Name).SendAsync("DisplayStartTime", TeamModel.DailyMeeting?.Date.ToShortTimeString());
        }

        public async Task DisplayTimer()
        {
            //na szutke czas
            var endTime = TeamModel.DailyMeeting?.Date.AddMinutes(1).Ticks;
            var nowTime = DateTime.Now.Ticks;

            var time = (endTime - nowTime) / 10000000;

            await Clients.Caller.SendAsync("DisplayTimer", TeamModel.IsDailyStarted, time);
        }

        public async Task SetUpTimer()
        {
            var endTime = TeamModel.DailyMeeting?.Date.AddMinutes(1).Ticks;
            var nowTime = DateTime.Now.Ticks;

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

            if (test.AddMinutes(1) < DateTime.Now)
            {
                if (TeamModel?.DailyMeeting != null && TeamModel.IsDailyStarted == true)
                {
                    await EndDailyMeeting();
                }

            }
        }


        //public async Task EndDailyMeetingByTime()
        //{
        //    await Task.Delay(10000);

        //    await EndDailyMeeting();
        //}

        // jest miodzio
        //public async Task<IOb> EndDailyMeetingTimer(TimeSpan timeSpan)
        //{
        //return Observable.Interval(timeSpan).Subscribe(async end => await EndDailyMeeting());

        ////var dummyTask = Task.Run(() =>
        ////{
        ////    Thread.Sleep(timeSpan);
        ////});


        ////Task.WaitAll(dummyTask);

        //var dummyTask = Task.Run(async() =>
        //{
        //    await Task.Delay(timeSpan);
        //    await EndDailyMeeting();
        //});

        ////await Task.Delay(timeSpan);

        //return dummyTask;
        //}
        //}
    }
}
