using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using System.Threading.Tasks;

namespace PocWebApp {
    public static class Program {
        public static async Task<int> Main(string[] args) {
            try {
                var hostBuilder = CreateHostBuilder(args);
                if (global::UpToDocu.Swagger.SwaggerGenerator.Generate(
                    args: args,
                    hostBuilder: hostBuilder,
                    swaggerOptions: new UpToDocu.Swagger.SwaggerOptions(),
                    configureForSwaggerGeneration: hostBuilder => {
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