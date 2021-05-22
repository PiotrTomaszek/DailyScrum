using DailyScrum.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Repository
{
    public interface ITeamRepository
    {
        DateTime GetDailyTime(string teamName);

        void CreateNewTeam(string teamName, DateTime dailyTime, string thisUserUserName);

        ApplicationUser AddNewTeamMember(string userName, string teamName);

        void DeleteTeam(string teamName);
    }
}
