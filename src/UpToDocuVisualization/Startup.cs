using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UpToDocuVisualization {
    public class Startup {
        public Startup(IConfiguration configuration) {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            var mvcBuilderControllers = services.AddControllers((Microsoft.AspNetCore.Mvc.MvcOptions options) => {
                options.RespectBrowserAcceptHeader = true;
            });
            var mvcBuilderRazor = services.AddRazorPages((Microsoft.AspNetCore.Mvc.RazorPages.RazorPagesOptions options) => {
                //options.Conventions.
            });
            /*
            mvcBuilderControllers.AddOData((odataOptions) => {
                odataOptions.EnableQueryFeatures().Select().Filter().OrderBy();
                var edmModel = UpToDocuVisualization.Model.EdmModelGenerator.GetEdmModel();
                odataOptions.AddRouteComponents("odata", edmModel);
            });
            */
            services.AddSwaggerGen(c => {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo {
                        Title = "UpToDocuVisualization",
                        Version = "v1"
                    });
                // later c.SchemaFilter<SwaggerSchemaFilter>();
                c.SupportNonNullableReferenceTypes();
                //c.SchemaFilter<EnumNamesSchemaFilter>();
                //c.DocumentFilter<DangerousSchemasDocumentFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapRazorPages();
            });
        }
    }
}
