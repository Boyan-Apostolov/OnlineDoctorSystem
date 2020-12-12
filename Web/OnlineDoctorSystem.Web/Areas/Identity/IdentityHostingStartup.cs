using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(OnlineDoctorSystem.Web.Areas.Identity.IdentityHostingStartup))]

namespace OnlineDoctorSystem.Web.Areas.Identity
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
