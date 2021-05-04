using System;
using DailyScrum.Areas.Identity.Data;
using DailyScrum.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(DailyScrum.Areas.Identity.IdentityHostingStartup))]
namespace DailyScrum.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<DailyScrumContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("DailyScrumContextConnection")));

                services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<DailyScrumContext>();
            });
        }
    }
}