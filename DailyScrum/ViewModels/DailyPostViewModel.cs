using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.ViewModels
{
    public class DailyPostViewModel
    {
        public string Id { get; set; }
        public string FromUser { get; set; }
        public DateTime Date { get; set; }


        public string FirstQuestion { get; set; }
        public string SecondQuestion { get; set; }
        public string ThirdQuestion { get; set; }

    }
}
