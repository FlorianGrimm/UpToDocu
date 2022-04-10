using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Poc;
using Poc.Entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UpToDocu;

namespace PocWebApp {
    public class Startup {
        public Startup(IConfiguration configuration) {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            this.ConfigureServicesCommon(services);
        }
        public void ConfigureSwaggerGeneratorServices(IServiceCollection services) {
            this.ConfigureServicesCommon(services);
        }
        public void ConfigureServicesCommon(IServiceCollection services) {
            services.AddDbContext<PocContext>(
                optionsAction: options => {
                    var todoConnectionString = this.Configuration.GetConnectionString("ToDo");
                    if (string.IsNullOrEmpty(todoConnectionString)) {
                        options.UseInMemoryDatabase("Todo");
                    } else { 
                        options.UseSqlServer(todoConnectionString, options => {
                            options.MigrationsAssembly("PocWebApp");
                        });
                    }
                },
                contextLifetime: ServiceLifetime.Transient);

            services.AddControllers();
            services.AddRazorPages();
            services.AddUpToDocu(options => {
                options.AddSpecification(PocWebApp.Spec.SpecificationPocWebApp.Instance);
            });

            services.AddRepository();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // app.UseAuthentication();
            // app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapRazorPages();
            });
        }
    }
}
