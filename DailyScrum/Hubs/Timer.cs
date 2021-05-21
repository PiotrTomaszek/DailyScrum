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
            await Clients.Caller.SendAsync("DisplayStartTime", TeamModel.DailyMeeting?.Date.ToShortTimeString());
        }


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
