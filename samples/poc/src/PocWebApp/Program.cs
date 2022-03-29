using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using System.Threading.Tasks;

namespace PocWebApp {
    public static class Program {
        public static async Task Main(string[] args) {
            IHostBuilder hostBuilder = CreateHostBuilder(args);
            using var host = hostBuilder.Build();

            await PocWebApp.HostingAbstractionsHostExtensions.RunAsync(host);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {
            var hostBuilder = Host.CreateDefaultBuilder(args);
            hostBuilder.ConfigureWebHostDefaults(webBuilder => {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseKestrel((Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions kestrelServerOptions) => {
                    //kestrelServerOptions.ApplicationServices
                });
            });
            return hostBuilder;
        }
    }
}
/*

dotnet-aspnet-codegenerator razorpage --project PocWebApp6.csproj --model ToDoEntity --dataContext PocDBContext --relativeFolderPath Pages
 
dotnet ef migrations add v1 --msbuildprojectextensionspath ..\..\output\PocWebApp6\obj

dotnet ef database update --msbuildprojectextensionspath ..\..\output\PocWebApp6\obj
 
 */