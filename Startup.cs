using ImageUpload.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageUpload.Data;

namespace ImageUpload
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
            services.AddControllersWithViews();
            services.AddDbContextPool<ImageDbContext>(
                 dbContextOptions => dbContextOptions
                     .UseMySql(
                         // Replace with your connection string.
                         "server=localhost;userid=root;password=Aksa@12345;database=ImageUpload",
                         // Replace with your server version and type.
                         // For common usages, see pull request #1233.
                         new MySqlServerVersion(new Version(8, 0, 21)), // use MariaDbServerVersion for MariaDB
                         mySqlOptions => mySqlOptions
                             .CharSetBehavior(CharSetBehavior.NeverAppend))
                     // Everything from this point on is optional but helps with debugging.
                     .EnableSensitiveDataLogging()
                     .EnableDetailedErrors()
          
    );

            services.AddDbContext<ImageUploadContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("ImageUploadContext")));

     
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
