using DailyScrum.Areas.Identity.Data;
using System.Collections.Generic;

namespace DailyScrum.ViewModels
{
    public class TeamViewModel
    {
        public int ConnectedUsersCount { get; set; }
        public int TeamMemberCount { get; set; }

        public IEnumerable<bool> UsersOnline { get; set; }
        public List<ApplicationUser> UsersList { get; set; }
    }
}
