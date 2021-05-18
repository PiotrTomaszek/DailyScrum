using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyScrum.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DailyScrum.Areas.Identity.Pages.Account.Manage
{
    public class ScrumOptionsModel : PageModel
    {
        public DailyScrumContext _context;

        public ScrumOptionsModel(DailyScrumContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            //var 

            //var user = _context.Users.Where()
        }
    }
}
