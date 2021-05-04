using DailyScrum.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Models.Database
{
    public class ScrumTask
    {
        public int ScrumTaskId { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }

        public ApplicationUser Executor { get; set; }
        public ScrumTaskBoard Board { get; set; }
    }
}
