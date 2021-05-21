using DailyScrum.Areas.Identity.Data;
using DailyScrum.Models.Database;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DailyScrum.ViewModels
{
    public class TeamViewModel
    {
        public int ConnectedUsersCount { get; set; }
        public int TeamMemberCount { get; set; }

        public bool IsDailyStarted { get; set; }
        public DailyMeeting DailyMeeting { get; set; }

        public DateTime MeetingStartingTime { get; set; }

        //tutaj uwaga
        public List<bool> UsersOnline { get; set; }
        // do wyrzucenia raczej nie
        public List<ApplicationUser> UsersList { get; set; }
        public List<NotificationViewModel> UsersNotification { get; set; }


        //// do wyrzucenia
        public List<MessageViewModel> Messages { get; set; }
    }
}
