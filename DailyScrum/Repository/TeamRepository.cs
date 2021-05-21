using DailyScrum.Areas.Identity.Data;
using DailyScrum.Data;
using DailyScrum.Models.Database;
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

        public void CreateNewTeam(string teamName, DateTime dailyTime, string thisUserUserName)
        {
            var creator = _context.Users
             .Include(r => r.TeamRole)
             .Include(n => n.TeamMember)
             .Where(x => x.UserName == thisUserUserName)
             .FirstOrDefault();

            try
            {
                var team = new Team
                {
                    DisplayName = teamName,
                    Name = Guid.NewGuid().ToString(),
                    DailyTime = dailyTime
                };

                _context.Teams.Add(team);

                var role = new ScrumRole
                {
                    Name = "Scrum Master"
                };

                _context.ScrumRoles.Add(role);
                _context.SaveChanges();

                creator.TeamRole = role;

                creator.TeamMember = team;

                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
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
