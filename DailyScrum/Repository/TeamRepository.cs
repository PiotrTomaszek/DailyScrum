using DailyScrum.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Repository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly DailyScrumContext _context;

        public TeamRepository(DailyScrumContext context)
        {
            _context = context;
        }
        public DateTime GetDailyTime(string teamName)
        {
            var time = _context.Teams
                .FirstOrDefault(r => r.Name.Equals(teamName));

            return time.DailyTime;
        }
    }
}
