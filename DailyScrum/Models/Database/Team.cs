using DailyScrum.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Models.Database
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public DateTime DailyTime { get; set; }

        public IEnumerable<ApplicationUser> Members { get; set; }
        public IEnumerable<DailyMeeting> Meetings { get; set; }

    }
}
