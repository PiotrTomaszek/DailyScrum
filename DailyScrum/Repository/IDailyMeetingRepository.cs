using DailyScrum.Models.Database;
using System.Collections.Generic;

namespace DailyScrum.Repository
{
    public interface IDailyMeetingRepository
    {
        List<DailyMeeting> GetAllMeetings(string userName);
        DailyMeeting GetMeeting(string userName, int meetingId);
        DailyMeeting CreateDailyMeeting(int teamId);
        void EndDailyMeeting(int meetingId);
    }
}
