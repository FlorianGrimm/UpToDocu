using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Writers;

using Swashbuckle.AspNetCore.Swagger;

namespace UpToDocuVisualization.Swagger {
    public static class SwaggerGenerator {
        public static bool Run(
            string[] args,
            Microsoft.Extensions.Hosting.IHostBuilder hostBuilder
            ) {
            var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
            configurationBuilder.Add(new Microsoft.Extensions.Configuration.CommandLine.CommandLineConfigurationSource() { Args = args });
            configurationBuilder.Add(new Microsoft.Extensions.Configuration.EnvironmentVariables.EnvironmentVariablesConfigurationSource());
            var configuration = configurationBuilder.Build();
            var swaggerDoc = configuration.GetValue<string>("swaggerDoc");
            var output = configuration.GetValue<string>("output");
            var yaml = configuration.GetValue<bool>("yaml");
            var serializeasv2 = configuration.GetValue<bool>("serializeasv2");
            var host = configuration.GetValue<string>("host");
            var basepath = configuration.GetValue<string>("basepath");
            if (!string.IsNullOrEmpty(swaggerDoc)) {
                //swaggerOutput
                var serviceProvider = hostBuilder.Build().Services;
                var swaggerProvider = serviceProvider.GetRequiredService<ISwaggerProvider>();
                var swagger = swaggerProvider.GetSwagger(swaggerDoc, host, basepath);
                var outputPath = !string.IsNullOrEmpty(output)
                        ? System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), output)
                        : null;

                using (var streamWriter = (!string.IsNullOrEmpty(outputPath) ? System.IO.File.CreateText(outputPath) : Console.Out)) {
                    IOpenApiWriter writer;
                    if (yaml) {
                        writer = new OpenApiYamlWriter(streamWriter);
                    } else {
                        writer = new OpenApiJsonWriter(streamWriter);
                    }

                    if (serializeasv2) {
                        swagger.SerializeAsV2(writer);
                    } else {
                        swagger.SerializeAsV3(writer);
                    }

                    if (outputPath != null) {
                        Console.WriteLine($"Swagger JSON/YAML successfully written to {outputPath}");
                    }
                }
                return false;
            } else {
                return true;
            }
        }
    }
}
