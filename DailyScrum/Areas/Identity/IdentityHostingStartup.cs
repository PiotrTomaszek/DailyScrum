using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(DailyScrum.Areas.Identity.IdentityHostingStartup))]
namespace DailyScrum.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}