using DailyScrum.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyScrum.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DailyScrumContext _context;

        public UserRepository(DailyScrumContext context)
        {
            _context = context;
        }

        public int GetTeamId(string userName)
        {
            var user = _context.Users
                .Include(a => a.TeamMember)
                .Where(x => x.UserName.Equals(userName))
                .FirstOrDefault();

            return user.TeamMember.TeamId;
        }

        public int GetTeamRoleId(string userName)
        {
            throw new NotImplementedException();
        }

        public void SetFirstName(string userName, string firstName)
        {
            var user = _context.Users
               .Where(x => x.UserName.Equals(userName))
               .FirstOrDefault();

            user.FirstName = firstName;

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void SetLastName(string userName, string lastName)
        {
            var user = _context.Users
                .Where(x => x.UserName.Equals(userName))
                .FirstOrDefault();

            user.LastName = lastName;

            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
