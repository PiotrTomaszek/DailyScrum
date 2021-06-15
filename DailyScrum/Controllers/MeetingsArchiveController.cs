using DailyScrum.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DailyScrum.Controllers
{
    [Authorize]
    public class MeetingsArchiveController : Controller
    {
        private string IdentityName => User.Identity.Name;

        private readonly IDailyMeetingRepository _dailyrepository;
        private readonly IUserRepository _userRepository;

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
                return Redirect("NoNoNo");
            }

            return View(_dailyrepository.GetAllMeetings(IdentityName));
        }

        public IActionResult Details(int id)
        {
            if (!_userRepository.CheckIfScrumMaster(IdentityName))
            {
                return RedirectToAction("NoNoNo");
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
