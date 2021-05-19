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
        private readonly IUserRepository _userRepository;

        private string IdentityName => User.Identity.Name;

        public MeetingsArchiveController(
            IDailyMeetingRepository repository,
            IUserRepository userRepository)
        {
            _dailyrepository = repository;
            _userRepository = userRepository;
        }


        public IActionResult Index()
        {
            if (!_userRepository.CheckIfScrumMaster(IdentityName))
            {
                return Redirect("/nonono");
            }

            return View(_dailyrepository.GetAllMeetings(IdentityName));
        }

        public IActionResult Details(int id)
        {
            if (!_userRepository.CheckIfScrumMaster(IdentityName))
            {
                return Redirect("/nonono");
            }

            var model = _dailyrepository.GetMeeting(IdentityName, id);

            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}
