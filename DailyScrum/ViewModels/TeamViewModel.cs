using DailyScrum.Areas.Identity.Data;
using DailyScrum.Models.Database;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DailyScrum.ViewModels
{
    public class TeamViewModel
    {
        public int ConnectedUsersCount { get; set; }
        public int TeamMemberCount { get; set; }

        public bool IsDailyStarted { get; set; }
        public DailyMeeting DailyMeeting { get; set; }

        public Timer Timer { get; set; }

        public DateTime MeetingStartingTime { get; set; }
        public TimeSpan MeettingDuration { get; set; }


        //tutaj uwaga
        public List<bool> UsersOnline { get; set; }
        // do wyrzucenia
        public List<ApplicationUser> UsersList { get; set; }


        //// do wyrzucenia
        public List<MessageViewModel> Messages { get; set; }
    }
}
