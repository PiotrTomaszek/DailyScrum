using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyScrum.Areas.Identity.Data;
using DailyScrum.Models.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DailyScrum.Data
{
    public class DailyScrumContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Team> Teams { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<DailyMeeting> DailyMeetings { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Role> ScrumRoles{ get; set; }

        public DailyScrumContext(DbContextOptions<DailyScrumContext> options)
            : base(options)
        {
        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //}
    }
}
