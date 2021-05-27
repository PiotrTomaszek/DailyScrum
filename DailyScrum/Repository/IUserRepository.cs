using DailyScrum.Areas.Identity.Data;
using System.Collections.Generic;

namespace DailyScrum.Repository
{
    public interface IUserRepository
    {
        int GetTeamId(string userName);
        int GetTeamRoleId(string userName);
        string GetUserPhotoPath(string userName);
        ApplicationUser GetUserByUserName(string userName);
        List<ApplicationUser> GetAllTeamMebers(string teamName);

        bool CheckIfScrumMaster(string userName);
        bool CheckIfHasTeam(string userName);
        bool CheckIfExists(string userName);

        ApplicationUser FindScrumMaster(int teamId);

        void SetFirstName(string userName, string firstName);
        void SetLastName(string userName, string lastName);
        ScrumRole SetTeamRole(string userName, string roleName);
        void SetPhotoPath(string userName, string photoLink);
        void SetPhotoPathById(string userId, string photoLink);
        string GetPhotoPathById(string userName);

    }
}
