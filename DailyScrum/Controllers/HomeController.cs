using DailyScrum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

        public IActionResult UserLogout()
        {
            return RedirectToPage("/Account/Logout", new { area = "Identity" });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
