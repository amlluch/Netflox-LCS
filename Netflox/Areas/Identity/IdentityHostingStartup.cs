using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Netflox.Areas.Identity.Data;
using Netflox.Models;

[assembly: HostingStartup(typeof(Netflox.Areas.Identity.IdentityHostingStartup))]
namespace Netflox.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<NetfloxContext>(options =>
                    options.UseMySql(
                        context.Configuration.GetConnectionString("NetfloxContextConnection")));

                //services.AddDefaultIdentity<NetfloxUser>()
                //    .AddEntityFrameworkStores<NetfloxContext>();
                services.AddDefaultIdentity<NetfloxUser>(config =>
                {
                    config.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores <NetfloxContext> ();
            });
        }
    }
}