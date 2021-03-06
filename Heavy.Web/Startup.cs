﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Heavy.Web.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Heavy.Web.Models;
using Heavy.Web.Auth;
using Microsoft.AspNetCore.Authorization;
using Heavy.Web.Services;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Heavy.Web.Filters;

namespace Heavy.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>(options=> {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                })
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("仅限管理员", policy => policy.RequireRole("Administrators"));
                options.AddPolicy("编辑", policy => policy.RequireClaim("Edit Role"));
                options.AddPolicy("编辑1", policy => policy.RequireAssertion(context =>
                {
                    if (context.User.HasClaim(x => x.Type == "Edit User"))
                        return true;
                    return false;
                }));
                options.AddPolicy("编辑2", policy => policy.AddRequirements(
                    new EmailRequirement("Email"),
                    new QualifiedUserRequirement()));

            });

            services.AddDbContext<HeavyContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IAlbumService, AlbumEfService>();

            services.AddSingleton<IAuthorizationHandler, EmailHandler>();
            services.AddSingleton<IAuthorizationHandler, CanEditAlbumHandler>();
            services.AddSingleton<IAuthorizationHandler, AdministratorsHandler>();

            services.AddAntiforgery(options =>
            {
                options.FormFieldName = "AntiforgeryFiledName";
                options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
                options.SuppressXFrameOptionsHeader = false;
            });

            services.AddMvc(options=> {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                //下面三种实现都一下 习惯第三种
                //options.Filters.Add(new LogResourceFilter());
                //options.Filters.Add(typeof(LogAsyncResourceFilter));
                options.Filters.Add<LogResourceFilter>();

                options.CacheProfiles.Add("Default", new CacheProfile
                {
                    Duration = 60
                });
                options.CacheProfiles.Add("Never", new CacheProfile
                {
                    Location = ResponseCacheLocation.None,
                    NoStore = true
                });
            });

            //services.AddMemoryCache();
            services.AddDistributedRedisCache(options=>{
                options.Configuration = "localhost";
                options.InstanceName = "redis-for-albums";
            });

            //压缩
            services.AddResponseCompression();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
           //app.UseWelcomePage();//网站未建设好提示页

            if (env.IsDevelopment())//开发模式
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseExceptionHandler("/Home/MyError");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseResponseCompression();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

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
