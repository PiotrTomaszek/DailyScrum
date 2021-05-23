using DailyScrum.Data;
using DailyScrum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly DailyScrumContext _context;
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger,
            DailyScrumContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var user = _context.Users
                .Include(x => x.TeamMember)
                .Where(u => u.UserName == User.Identity.Name)
                .FirstOrDefault();

            if (user?.TeamMember == null)
            {
                return RedirectToAction(nameof(UserWithoutTeam));
            }

            return View();
        }

        [Route("/chat")]
        public IActionResult Chat()
        {
            var user = _context.Users
                .Include(x => x.TeamMember)
                .Where(u => u.UserName == User.Identity.Name)
                .FirstOrDefault();

            if (user?.TeamMember == null)
            {
                return RedirectToAction(nameof(UserWithoutTeam));
            }

            return View();
        }


        [Route("/creator")]
        public IActionResult Creator()
        {
            return View();
        }

        [Route("/noteam")]
        public IActionResult UserWithoutTeam()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RedirectToAccount()
        {
            return Redirect("/konto");
        }

        // to na razie nie dzioa
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
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // nie dziala 
        //public async Task UploadFileToFile(IFormFile file)
        //{
        //    if (file == null)
        //    {
        //        HttpContext.Session.SetString("ImageName", "");
        //        return;
        //    }

        //    var uniqueName = $"{Guid.NewGuid()}_{file.FileName}";
        //    HttpContext.Session.SetString("ImageName", uniqueName);

        //    var toFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
        //    var filePath = Path.Combine(toFolder, uniqueName);

        //    using var fileStream = new FileStream(filePath, FileMode.Create);
        //    await file.CopyToAsync(fileStream);
        //}
    }
}
