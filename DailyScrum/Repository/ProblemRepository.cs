using DailyScrum.Data;
using DailyScrum.Models.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DailyScrum.Repository
{
    public class ProblemRepository : IProblemRepository
    {
        private readonly DailyScrumContext _context;

        public ProblemRepository(DailyScrumContext context)
        {
            _context = context;
        }

        public Problem CreateProblem(int teamId, int meetingId, string userId, string problemContent)
        {
            var newProblem = new Problem
            {
                Fixed = false,
                Description = problemContent,
                FromUser = _context.Users.Find(userId),
                Meeting = _context.DailyMeetings.Find(meetingId)
            };

            _context.Problems.Add(newProblem);
            _context.SaveChanges();

            return _context.Problems.Find(newProblem.ProblemId);
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
                .Include(z => z.FromUser)
                .Where(a => allMeetings.Contains(a.Meeting.DailyMeetingId) && a.Fixed == false)
                .ToList();

            return allProblems;
        }

        public void CompleteProblem(int id)
        {
            var problem = _context.Problems
                .Include(z => z.FromUser)
                .Where(a => a.ProblemId == id)
                .FirstOrDefault();

            if (problem != null)
            {
                problem.Fixed = true;

                _context.Update(problem);
                _context.SaveChanges();
            }
        }
    }
}
