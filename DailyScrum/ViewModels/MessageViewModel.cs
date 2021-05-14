using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.ViewModels
{
    public class MessageViewModel
    {
        public string Id { get; set; }
        public string From { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}
