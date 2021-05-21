using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DailyScrum.Areas.Identity.Data;
using DailyScrum.Data;
using DailyScrum.Models.Database;
using DailyScrum.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DailyScrum.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class ScrumOptionsModel : PageModel
    {
        public IUserRepository _userRepository;
        public DailyScrumContext _context;

        public ScrumOptionsModel(DailyScrumContext context,
            IUserRepository userRepo)
        {
            _context = context;
            _userRepository = userRepo;
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
                CreateNewTeam(Input.Name, Input.DailyTime);

                return RedirectToPage("./ScrumOptions");
            }

            SetViewData();
            return Page();
        }

        public void CreateNewTeam(string teamName, DateTime dailyTime)
        {
            var thisUserUserName = HttpContext.User.Identity.Name;

            var creator = _context.Users
               .Include(r => r.TeamRole)
               .Include(n => n.TeamMember)
               .Where(x => x.UserName == thisUserUserName)
               .FirstOrDefault();

            try
            {
                var team = new Team
                {
                    DisplayName = teamName,
                    Name = Guid.NewGuid().ToString(),
                    DailyTime = dailyTime
                };

                _context.Teams.Add(team);

                var role = new ScrumRole
                {
                    Name = "Scrum Master"
                };

                _context.ScrumRoles.Add(role);
                _context.SaveChanges();

                creator.TeamRole = role;

                creator.TeamMember = team;

                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

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
            if (_userRepository.CheckIfScrumMaster(User.Identity.Name))
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
