using DailyScrum.Models.Database;
using System.Collections.Generic;

namespace DailyScrum.Repository
{
    public interface IDailyMeetingRepository
    {
        DailyMeeting CreateDailyMeeting(int teamId);
        void EndDailyMeeting(int meetingId);
        DailyMeeting GetMeeting(string userName, int meetingId);
        List<DailyMeeting> GetAllMeetings(string userName);
    }
}
