using DailyScrum.Areas.Identity.Data;
using System.Collections.Generic;

namespace DailyScrum.ViewModels
{
    public class TeamViewModel
    {
        public int ConnectedUsersCount { get; set; }
        public int TeamMemberCount { get; set; }

        public List<bool> UsersOnline { get; set; }
        public List<ApplicationUser> UsersList { get; set; }

        public List<MessageViewModel> Messages { get; set; }
    }
}
