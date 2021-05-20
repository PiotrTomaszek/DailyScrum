using DailyScrum.Areas.Identity.Data;
using DailyScrum.Models.Database;
using System;
using System.Collections.Generic;

namespace DailyScrum.Repository
{
    public interface IPostRepository
    {
        List<DailyPost> GetAllPost(int meetingId);

        bool HasAlreadyPosted(string userName, int? meetingId);

        DailyPost CreateDailyPost(string first, string second, string third, ApplicationUser from, DailyMeeting daily, DateTime date);
    }
}
