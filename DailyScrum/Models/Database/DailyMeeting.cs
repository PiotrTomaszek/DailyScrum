using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Models.Database
{
    public class DailyMeeting
    {
        public int DailyMeetingId { get; set; }
        public Team Team { get; set; }

        [Display(Name = "Data spotkania")]
        public DateTime Date { get; set; }
        [Display(Name = "Zakończono")]
        public bool HasFinished { get; set; }

        public IEnumerable<DailyPost> Posts { get; set; }

        public IEnumerable<Problem> Problems { get; set; }
    }
}
