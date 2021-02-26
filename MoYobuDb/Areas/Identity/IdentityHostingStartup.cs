using Microsoft.AspNetCore.Hosting;
using MoYobuDb.Areas.Identity;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]

namespace MoYobuDb.Areas.Identity
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