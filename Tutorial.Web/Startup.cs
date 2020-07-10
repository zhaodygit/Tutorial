using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System.IO;
using Tutorial.Web.Data;
using Tutorial.Web.Model;
using Tutorial.Web.Services;

namespace Tutorial.Web
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            var connectionString = configuration["ConnectionStrings:DefaultConnection"];
            connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            services.AddSingleton<IWelcomeService, WelcomeServie>();
            services.AddScoped<IRepository<Student>, EFCoreRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            IWelcomeService welcomeServie,
            ILogger<Startup> logger
            )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();//非开发
            }
            //app.Use(next =>
            //{
            //    logger.LogInformation("app.Use().............");
            //    return async HttpContext =>
            //    {
            //        if (HttpContext.Request.Path.StartsWithSegments("/first"))
            //        {
            //            logger.LogInformation(".............first");
            //            await HttpContext.Response.WriteAsync("First!!");
            //        }
            //        else {
            //            logger.LogInformation(".............next(HttpContext)");
            //            await next(HttpContext);
            //        }
            //    };
            //});

            //app.UseWelcomePage(new WelcomePageOptions {
            //    Path = "/welcome"
            //});
            //app.UseDefaultFiles();//打开默认首页
            app.UseStaticFiles();//静态文件伺服
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/node_modules",
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "node_modules"))
            });

            //app.UseFileServer();//包含上面两个

            app.UseMvcWithDefaultRoute();

            //
            //app.Run(async (context) =>
            //{
            //    var welcome = welcomeServie.getMessage();
            //    await context.Response.WriteAsync(welcome);
            //});
        }
    }
}
