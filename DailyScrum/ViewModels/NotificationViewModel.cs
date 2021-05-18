using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.ViewModels
{
    public class NotificationViewModel
    {
        public bool MeetingNotify { get; set; }
        public bool ChatNotify { get; set; }
        public bool ProblemNotify { get; set; }

    }
}
