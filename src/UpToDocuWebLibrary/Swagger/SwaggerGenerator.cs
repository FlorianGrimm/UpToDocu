using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Writers;

using Swashbuckle.AspNetCore.Swagger;

using System;

namespace UpToDocu.Swagger {
    /// <summary>
    /// swagger:documentname - as defined in code otherwise run webapp.
    /// swagger:output - output filepath otherwise Console.Out.
    /// swagger:yaml true:yaml otherwise json.
    /// swagger:serializeasv2 - true:v2 otherwise v3.
    /// swagger:host - host in swagger.
    /// swagger:basepath - basepath in swagger.
    /// </summary>
    public static class SwaggerGenerator {
        public static bool Generate(
            string[] args,
            Microsoft.Extensions.Hosting.IHostBuilder hostBuilder,
            Action<Microsoft.Extensions.Hosting.IHostBuilder>? configureForSwaggerGeneration = default
            ) {
            var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
            configurationBuilder.Add(new Microsoft.Extensions.Configuration.CommandLine.CommandLineConfigurationSource() { Args = args });
            configurationBuilder.Add(new Microsoft.Extensions.Configuration.EnvironmentVariables.EnvironmentVariablesConfigurationSource() { Prefix = "swagger:" });
            var configuration = configurationBuilder.Build();

            var swaggerDocumentName = configuration.GetValue<string>("swagger:documentname");

            if (!string.IsNullOrEmpty(swaggerDocumentName)) {
                var output = configuration.GetValue<string>("swagger:output");
                var yaml = configuration.GetValue<bool>("swagger:yaml");
                var serializeasv2 = configuration.GetValue<bool>("swagger:serializeasv2");
                var host = configuration.GetValue<string>("swagger:host");
                var basepath = configuration.GetValue<string>("swagger:basepath");

                if (configureForSwaggerGeneration is not null) {
                    configureForSwaggerGeneration(hostBuilder);
                }
                var serviceProvider = hostBuilder.Build().Services;
                var swaggerProvider = serviceProvider.GetRequiredService<ISwaggerProvider>();
                var swagger = swaggerProvider.GetSwagger(swaggerDocumentName, host, basepath);
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
                        System.Console.Out.WriteLine($"Swagger {(serializeasv2 ? "V2" : "V3")} {(yaml ? "YAML" : "JSON")} successfully written to {outputPath}");
                    }
                }
                return true;
            } else {
                return false;
            }
        }
    }
}
