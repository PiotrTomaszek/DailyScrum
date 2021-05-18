using DailyScrum.Models.Database;
using System.Collections.Generic;

namespace DailyScrum.Repository
{
    public interface IDailyMeetingRepository
    {
        List<DailyMeeting> GetAllMeetings(string userName);
        DailyMeeting GetMeeting(string userName, int meetingId);
        void CreateDailyMeeting(int teamId);
    }
}
