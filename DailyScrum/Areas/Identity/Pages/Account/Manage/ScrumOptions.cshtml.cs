using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyScrum.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

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
            var thisUserUserName = HttpContext.User.Identity.Name;

            var user = _context.Users
                .Include(r => r.TeamRole)
                .Include(n => n.TeamMember)
                .Where(x => x.UserName == thisUserUserName)
                .FirstOrDefault();

            ViewData["MyNumber"] = 42;

            if (user.TeamMember == null && user.TeamRole == null)
            {
                ViewData["HasTeam"] = false;
            }
            else
            {
                ViewData["HasTeam"] = true;

                // do wyswietlenie dla Scrum Mastera
                if (user.TeamRole.RoleId == 1)
                {
                    ViewData["IsScrumMaster"] = true;
                }
                else
                {
                    ViewData["IsScrumMaster"] = false;
                }
            }
        }

        public void OnPost()
        {

        }
    }
}
