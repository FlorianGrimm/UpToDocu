using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UpToDocuVisualization {
    public static class Program {
        public static async Task<int> Main(string[] args) {
            try {
                var hostBuilder = CreateHostBuilder(args);
                if (global::UpToDocu.Swagger.SwaggerGenerator.Generate(
                    args:args, 
                    hostBuilder: hostBuilder, 
                    swaggerOptions: new UpToDocu.Swagger.SwaggerOptions(),
                    configureForSwaggerGeneration:hostBuilder => {
                    hostBuilder.UseEnvironment("SwaggerGenerator");
                })) {
                    System.Console.Out.WriteLine("SwaggerGenerator");
                    return 0;
                }
                {
                    System.Console.Out.WriteLine("Booting WebApp");
                    var host = hostBuilder.Build();
                    await UpToDocu.WebApp.UpToDocuHostExtensions.RunAsync(host);
                    return 0;
                }
            } catch (System.Exception error) {
                UpToDocu.Console.ExceptionExtensions.WriteError(error);
                return 1;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {
            var hostBuilder = Host.CreateDefaultBuilder(args);
            hostBuilder.ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
            return hostBuilder;
        }
    }
}
