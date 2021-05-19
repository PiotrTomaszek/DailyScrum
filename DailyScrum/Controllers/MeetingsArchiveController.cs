using DailyScrum.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Controllers
{
    [Authorize]
    public class MeetingsArchiveController : Controller
    {
        private readonly IDailyMeetingRepository _dailyrepository;

        private string IdentityName => User.Identity.Name;

        public MeetingsArchiveController(IDailyMeetingRepository repository)
        {
            _dailyrepository = repository;
        }


        public IActionResult Index()
        {
            return View(_dailyrepository.GetAllMeetings(IdentityName));
        }

        public IActionResult Details(int id)
        {
            var model = _dailyrepository.GetMeeting(IdentityName, id);

            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}
