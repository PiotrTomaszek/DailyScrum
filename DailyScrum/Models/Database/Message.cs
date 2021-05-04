using DailyScrum.Areas.Identity.Data;
using System;

namespace DailyScrum.Models.Database
{
    public class Message
    {
        public int MessageId { get; set; }

        public string Content { get; set; }
        public DateTime Date { get; set; }
        public ApplicationUser FromUser { get; set; }
        public string ToMeeting { get; set; }
    }
}
