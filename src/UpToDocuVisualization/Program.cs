using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UpToDocuVisualization {
    public class Program {
        public static void Main(string[] args) {
            var hostBuilder = CreateHostBuilder(args);
            if (Swagger.SwaggerGenerator.Run(args, hostBuilder)) {
                System.Console.Out.WriteLine("booting");
                //hostBuilder.Build().Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {
            var result = Host.CreateDefaultBuilder(args);
            result.ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
            return result;
        }
    }
}
