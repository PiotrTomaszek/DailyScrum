using DailyScrum.Data;
using DailyScrum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DailyScrumContext _context;


        public HomeController(ILogger<HomeController> logger, DailyScrumContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [Route("/chat")]
        public IActionResult Chat()
        {
            return View();
        }


        [Route("/creator")]
        public IActionResult Creator()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RedirectToAccount()
        {
            return Redirect("/konto");
        }

        //[HttpGet]
        //public async Task DeleteTeam()
        //{
        //    //return 
        //}

        [HttpPost]
        public async Task DeleteTeam()
        {
            var scrumMaster = _context.Users
                .Include(a => a.TeamMember)
                .Where(x => x.UserName == HttpContext.User.Identity.Name)
                .FirstOrDefault();

            var team = _context.Teams.FindAsync(scrumMaster.TeamMember.TeamId).Result;

            try
            {
                var toUpdate = _context.Users.Include(a => a.TeamMember).Where(x => x.TeamMember.TeamId == team.TeamId).ToList();

                scrumMaster.TeamRole = null;
                scrumMaster.TeamMember = null;

                await _context.SaveChangesAsync();
                _context.Users.Update(scrumMaster);
                await _context.SaveChangesAsync();

                foreach (var item in toUpdate)
                {
                    item.TeamRole = null;
                    item.TeamMember = null;

                    await _context.SaveChangesAsync();
                    _context.Update(item);
                }

                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

            //return RedirectToPage("");
        }

        //public IActionResult UserLogout()
        //{
        //    return RedirectToPage("/Account/Logout", new { area = "Identity" });
        //}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
