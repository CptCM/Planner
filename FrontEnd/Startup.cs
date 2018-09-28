using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using FrontEnd.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using FrontEnd.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace FrontEnd
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.Filters.AddService<RequireLoginFilter>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/admin", "Admin");
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                    options.AllowAreas = true;
                });

            //var authBuilder = services
            //    .AddAuthentication(options =>
            //    {
            //        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    })
            //    .AddCookie(options =>
            //    {
            //        options.LoginPath = $"/Identity/Account/Login";
            //        options.LogoutPath = $"/Identity/Account/Logout";
            //        options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            //    });




            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireAuthenticatedUser()
                          .RequireUserName(Configuration["admin"]);
                });
            });

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(Configuration["serviceUrl"])
            };
            services.AddSingleton(httpClient);
            services.AddSingleton<IApiClient, ApiClient>();
            services.AddTransient<RequireLoginFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();
<<<<<<< HEAD
=======
            app.UseHttpsRedirection();
>>>>>>> da47c9fa4eaf0da2d154e132e4804c210b667967
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
