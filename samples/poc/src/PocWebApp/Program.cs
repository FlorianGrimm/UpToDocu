using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocWebApp {
    public class Program {
        public static void Main(string[] args) {
            IHostBuilder hostBuilder = CreateHostBuilder(args);
            hostBuilder.Build().Run();
            hostBuilder.Build().RunAsync
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
/*

dotnet-aspnet-codegenerator razorpage --project PocWebApp6.csproj --model ToDoEntity --dataContext PocDBContext --relativeFolderPath Pages
 
dotnet ef migrations add v1 --msbuildprojectextensionspath ..\..\output\PocWebApp6\obj

dotnet ef database update --msbuildprojectextensionspath ..\..\output\PocWebApp6\obj
 
 */