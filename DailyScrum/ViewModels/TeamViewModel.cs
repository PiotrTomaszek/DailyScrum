using DailyScrum.Areas.Identity.Data;
using DailyScrum.Models.Database;
using System;
using System.Collections.Generic;

namespace DailyScrum.ViewModels
{
    public class TeamViewModel
    {
        public int ConnectedUsersCount { get; set; }
        public int TeamMemberCount { get; set; }

        public bool IsDailyStarted { get; set; }
        public DailyMeeting DailyMeeting { get; set; }

        public DateTime MeetingStartingTime { get; set; }

        public List<bool> UsersOnline { get; set; }
        public List<ApplicationUser> UsersList { get; set; }
        public List<NotificationViewModel> UsersNotification { get; set; }
    }
}
