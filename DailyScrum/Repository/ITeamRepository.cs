using DailyScrum.Areas.Identity.Data;
using System;

namespace DailyScrum.Repository
{
    public interface ITeamRepository
    {
        void CreateNewTeam(string teamName, DateTime dailyTime, string thisUserUserName);
        void DeleteTeam(string teamName);

        DateTime GetDailyTime(string teamName);

        ApplicationUser AddNewTeamMember(string userName, string teamName);
        ApplicationUser RemoveTeamMember(string userName);
    }
}
