using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Models.Database
{
    public class DailyMeeting
    {
        public int DailyMeetingId { get; set; }
        public Team Team { get; set; }
        public DateTime Date { get; set; }

        public IEnumerable<ScrumTask> Tasks { get; set; }
        public IEnumerable<Message> Messages { get; set; }
        public IEnumerable<Problem> Problems { get; set; }
    }
}
