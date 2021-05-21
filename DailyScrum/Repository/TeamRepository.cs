using DailyScrum.Data;
using Microsoft.EntityFrameworkCore;
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

        public void DeleteTeam(string teamName)
        {
            var team = _context.Teams
                .Where(x => x.Name.Equals(teamName))
                .FirstOrDefault();

            var members = _context.Users
                .Include(a => a.TeamRole)
                .Include(b => b.TeamMember)
                .Where(x => x.TeamMember.Name.Equals(team.Name))
                .ToList();

            foreach (var item in members)
            {
                item.TeamMember = null;

                _context.ScrumRoles.Remove(item.TeamRole);
                _context.SaveChanges();
            }

            //_context.Teams.Remove(team);

            _context.SaveChanges();
        }

        public DateTime GetDailyTime(string teamName)
        {
            var time = _context.Teams
                .FirstOrDefault(r => r.Name.Equals(teamName));

            return time.DailyTime;
        }
    }
}
