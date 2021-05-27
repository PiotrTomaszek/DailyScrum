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
        private DailyScrumContext _context;
        private IUserRepository _userRepository;
        private ITeamRepository _teamRepository;

        public ScrumOptionsModel(DailyScrumContext context,
            IUserRepository userRepo,
            ITeamRepository teamRepository)
        {
            _context = context;
            _userRepository = userRepo;
            _teamRepository = teamRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {
            [Required(ErrorMessage = "Nazwa zespo³u jest wymagana")]
            [DataType(DataType.Text)]
            [Display(Name = "Nazwa zespo³u")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Podaj odpowiedni¹ datê.")]
            [DataType(DataType.Time, ErrorMessage = "Z³e dane.")]
            [Display(Name = "Pora Daily")]
            public DateTime DailyTime { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            SetViewData();

            return Page();
        }

        public IActionResult OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                _teamRepository.CreateNewTeam(Input.Name, Input.DailyTime.ToUniversalTime(), HttpContext.User.Identity.Name);

                return RedirectToPage("./ScrumOptions");
            }

            SetViewData();
            return Page();
        }

        public void SetViewData()
        {
            var thisUserUserName = HttpContext.User.Identity.Name;

            var user = _context.Users
                .Include(r => r.TeamRole)
                .Include(n => n.TeamMember)
                .Where(x => x.UserName == thisUserUserName)
                .FirstOrDefault();

            if (user.TeamMember == null)
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
