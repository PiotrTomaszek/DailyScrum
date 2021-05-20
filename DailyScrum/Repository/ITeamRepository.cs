using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Repository
{
    public interface ITeamRepository
    {
        DateTime GetDailyTime(string teamName);
    }
}
