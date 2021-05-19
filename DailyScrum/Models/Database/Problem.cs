using DailyScrum.Areas.Identity.Data;

namespace DailyScrum.Models.Database
{
    public class Problem
    {
        public int ProblemId { get; set; }
        public string Description { get; set; }
        public bool Fixed { get; set; }

        public ApplicationUser FromUser { get; set; }

        public DailyMeeting Meeting { get; set; }
    }
}