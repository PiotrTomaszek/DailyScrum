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

        bool CheckIfScrumMaster(string userName);

        void SetFirstName(string userName, string firstName);
        void SetLastName(string userName, string lastName);
    }
}
