using DailyScrum.Areas.Identity.Data;
using DailyScrum.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Repository
{
    public interface IPostRepository
    {
        List<DailyPost> GetAllPost(int meetingId);

        DailyPost CreateDailyPost(string first, string second, string third, ApplicationUser from, DailyMeeting daily, DateTime date);
    }
}
