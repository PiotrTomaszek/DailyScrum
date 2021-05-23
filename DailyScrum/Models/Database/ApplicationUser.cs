using DailyScrum.Models.Database;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DailyScrum.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoPath { get; set; }

        public Team TeamMember { get; set; }

        public ScrumRole TeamRole { get; set; }
    }
}
