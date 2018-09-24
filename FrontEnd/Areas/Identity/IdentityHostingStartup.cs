using FrontEnd.Areas.Identity.Data;
using FrontEnd.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(FrontEnd.Areas.Identity.IdentityHostingStartup))]
namespace FrontEnd.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<FrontEndIdentityContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("FrontEndIdentityContextConnection")));

                services.AddDefaultIdentity<FrontEndUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<FrontEndIdentityContext>();

                Microsoft.AspNetCore.Authentication.AuthenticationBuilder authBuilder = services.AddAuthentication();

                IConfigurationSection twitterConfig = context.Configuration.GetSection("twitter");
                if (twitterConfig["consumerKey"] != null)
                {
                    authBuilder.AddTwitter(options => twitterConfig.Bind(options));
                }

                IConfigurationSection googleConfig = context.Configuration.GetSection("google");
                if (googleConfig["clientID"] != null)
                {
                    authBuilder.AddGoogle(options => googleConfig.Bind(options));
                }
            });
        }
    }
}