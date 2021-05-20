using DailyScrum.Areas.Identity.Data;
using System;

namespace DailyScrum.Models.Database
{
    public class DailyPost
    {
        public int DailyPostId { get; set; }
        public ApplicationUser FromUser { get; set; }
        public DailyMeeting Meeting { get; set; }

        public string FirstQuestion { get; set; }
        public string SecondQuestion { get; set; }
        public string ThirdQuestion { get; set; }

        public DateTime Date { get; set; }
    }
}