using DailyScrum.Data;
using DailyScrum.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Models.DbLogic
{
    public class TeamCrud : IDatabaseTeam
    {
        private DailyScrumContext _context;

        public TeamCrud(DailyScrumContext context)
        {
            _context = context;
        }

        public void CreateTeam(string name, DateTime dailyTime, string creatorName)
        {
            var thisUserUserName = creatorName;

            var creator = _context.Users
               .Include(r => r.TeamRole)
               .Include(n => n.TeamMember)
               .Where(x => x.UserName == thisUserUserName)
               .FirstOrDefault();

            try
            {
                var team = new Team
                {
                    DisplayName = name,
                    Name = Guid.NewGuid().ToString()
                };

                _context.Teams.Add(team);

                creator.TeamRole = _context.ScrumRoles.Find(1);
                creator.TeamMember = team;

                _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteTeam(string name)
        {
            throw new NotImplementedException();
        }

        public Team GetTeam(string name)
        {
            throw new NotImplementedException();
        }
    }
}
