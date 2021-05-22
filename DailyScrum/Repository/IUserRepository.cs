using DailyScrum.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Repository
{
    public interface IUserRepository
    {
        int GetTeamId(string userName);
        int GetTeamRoleId(string userName);


        ApplicationUser GetUserByUserName(string userName);
        List<ApplicationUser> GetAllTeamMebers(string teamName);

        bool CheckIfScrumMaster(string userName);
        bool CheckIfHasTeam(string userName);

        ApplicationUser FindScrumMaster(int teamId);


        void SetFirstName(string userName, string firstName);
        void SetLastName(string userName, string lastName);
        ScrumRole SetTeamRole(string userName, string roleName);
    }
}
