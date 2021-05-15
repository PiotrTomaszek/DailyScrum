using DailyScrum.Areas.Identity.Data;
using System;
using System.Collections.Generic;

namespace DailyScrum.ViewModels
{
    public class TeamViewModel
    {
        public int ConnectedUsersCount { get; set; }
        public int TeamMemberCount { get; set; }

        public TimeSpan MeetingStartingTime{ get; set; }
        public TimeSpan MeettingDuration { get; set; }

        public List<DailyPostViewModel> DailyPosts { get; set; }

        public List<bool> UsersOnline { get; set; }
        public List<ApplicationUser> UsersList { get; set; }
        public List<MessageViewModel> Messages { get; set; }
    }
}
