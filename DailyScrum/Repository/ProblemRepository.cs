using DailyScrum.Data;
using DailyScrum.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Repository
{
    public class ProblemRepository : IProblemRepository
    {
        private readonly DailyScrumContext _context;

        public ProblemRepository(DailyScrumContext context)
        {
            _context = context;
        }

        public IEnumerable<Problem> GetAllActiveProblems(int teamId)
        {
            var allMeetings = _context.DailyMeetings
                .Include(x => x.Problems)
                .Include(z => z.Team)
                .Where(daily => daily.Team.TeamId == teamId)
                .Select(a => a.DailyMeetingId)
                .ToList<int>();

            var allProblems = _context.Problems
                .Include(x => x.Meeting)
                .Where(a => allMeetings.Contains(a.Meeting.DailyMeetingId) && a.Fixed == false)
                .ToList();

            return allProblems;
        }
    }
}
