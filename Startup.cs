using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using WebAPIWithMySQL.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationMySQL
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

            var host = Configuration["DBHOST"] ?? "localhost";
            //var host =  "192.168.57.241";

            var port = Configuration["DBPORT"] ?? "3306";

            var password = Configuration["DBPASSWORD"] ?? "Current-Root-Password";



            services.AddDbContextPool<StudentDetailContext>(
                    options =>
                    {
                        options.UseMySql($"server={host};uid=root;pwd={password};"
                        + $"port={port};database=ehrscrapper");
                    });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, StudentDetailContext context)
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

            context.Database.Migrate();
            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=StudentDetails}/{action=Index}/{id?}");
            });
        }
    }
}
