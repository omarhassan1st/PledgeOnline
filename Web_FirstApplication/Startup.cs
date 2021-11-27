using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_FirstApplication.Conest;
using Web_FirstApplication.Const;
using Web_FirstApplication.Models.ViewModel;
using Web_FirstApplication.Repository.Declaration.IAccount;
using Web_FirstApplication.Repository.Declaration.IShard;
using Web_FirstApplication.Repository.Declaration.IWebSite;
using Web_FirstApplication.Repository.Implementation.Account;
using Web_FirstApplication.Repository.Implementation.Shard;
using Web_FirstApplication.Repository.Implementation.WebSite;

namespace Web_FirstApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //Models Configure
            services.AddTransient<LayoutViewModel, LayoutViewModel>();
            services.AddTransient<Services.Mailing, Services.Mailing>();

            services.Configure<Settings.Email>(Configuration.GetSection("EmailSettings:Gmail"));

            //WebSiteDbContext Configure
            services.AddDbContext<WebSiteDbBase>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("WebSite")));
            services.AddTransient<IWebSiteDbContext, WebSiteDbContext>();

            //AccountDbContext Configure
            services.AddDbContext<AccountDbBase>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Account")));
            services.AddTransient<IAccountDbContext, AccountDbContext>();

            //ShardDbContext Configure
            services.AddDbContext<ShardDbBase>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Shard")));
            services.AddTransient<IShardDbContext, ShardDbContext>();

            //Identity Configure
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<WebSiteDbBase>()
                .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>(TokenOptions.DefaultProvider);
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedAccount = false;
            });

            //Add Authorize
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/AccessDenied";
                options.LogoutPath = "/Identity/LogOut";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(15);
                options.LoginPath = "/Identity/Login";
                options.ReturnUrlParameter = "returnUrl";
                options.SlidingExpiration = false;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy =CookieSecurePolicy.Always;
            });

            //Add MVC
            services.AddControllersWithViews();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //Initialize Database
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var services = serviceScope.ServiceProvider.GetRequiredService<WebSiteDbBase>();
                services.Database.Migrate();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "areas",
                    areaName: "Admin",
                    pattern: "Admin/{controller=DownloadLinks}/{action=Index}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
