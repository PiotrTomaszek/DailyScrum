using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DailyScrum.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DailyScrum.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class ScrumOptionsModel : PageModel
    {
        public DailyScrumContext _context;

        public ScrumOptionsModel(DailyScrumContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {
            [Required(ErrorMessage = "Nazwa zespo³u jest wymagana")]
            [DataType(DataType.Text)]
            [Display(Name = "Nazwa zespo³u")]
            public string Name { get; set; }

            [DataType(DataType.Time, ErrorMessage = "Z³e dane.")]
            [Display(Name = "Pora Daily")]
            public DateTime DailyTime { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            SetViewData();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var testa = Input.Name;
            var test = Input.DailyTime;

            if (ModelState.IsValid)
            {


                return RedirectToPage("./ScrumOptions");
            }

            SetViewData();
            return Page();
        }

        public void CreateNewTeam()
        {
            var thisUserUserName = HttpContext.User.Identity.Name;

            var creator = _context.Users
               .Include(r => r.TeamRole)
               .Include(n => n.TeamMember)
               .Where(x => x.UserName == thisUserUserName)
               .FirstOrDefault();


        }

        public void SetViewData()
        {
            var thisUserUserName = HttpContext.User.Identity.Name;

            var user = _context.Users
                .Include(r => r.TeamRole)
                .Include(n => n.TeamMember)
                .Where(x => x.UserName == thisUserUserName)
                .FirstOrDefault();

            if (user.TeamMember == null && user.TeamRole == null)
            {
                ViewData["HasTeam"] = false;
            }
            else
            {
                ViewData["HasTeam"] = true;
            }

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
}
