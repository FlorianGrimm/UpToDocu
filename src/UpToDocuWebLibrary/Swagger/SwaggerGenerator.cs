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
            SwaggerOptions? swaggerOptions = default,
            Action<Microsoft.Extensions.Hosting.IHostBuilder>? configureForSwaggerGeneration = default
            ) {
            var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
            configurationBuilder.Add(new Microsoft.Extensions.Configuration.CommandLine.CommandLineConfigurationSource() { Args = args });
            configurationBuilder.Add(new Microsoft.Extensions.Configuration.EnvironmentVariables.EnvironmentVariablesConfigurationSource() { Prefix = "swagger:" });
            var configuration = configurationBuilder.Build();

            if (swaggerOptions is null) {
                swaggerOptions = new SwaggerOptions();
            }
            configuration.GetSection("Swagger").Bind(swaggerOptions);

            if (swaggerOptions.Generate) {
                //&& !string.IsNullOrEmpty(swaggerDocumentName)

                //var swaggerDocumentName = configuration.GetValue<string>("swagger:documentname");
                //var output = configuration.GetValue<string>("swagger:output");
                //var yaml = configuration.GetValue<bool>("swagger:yaml");
                //var serializeasv2 = configuration.GetValue<bool>("swagger:serializeasv2");
                //var host = configuration.GetValue<string>("swagger:host");
                //var basepath = configuration.GetValue<string>("swagger:basepath");

                if (configureForSwaggerGeneration is not null) {
                    configureForSwaggerGeneration(hostBuilder);
                }
                var serviceProvider = hostBuilder.Build().Services;
                var swaggerProvider = serviceProvider.GetRequiredService<ISwaggerProvider>();
                var swagger = swaggerProvider.GetSwagger(
                    documentName: swaggerOptions.DocumentName,
                    host: (string.IsNullOrEmpty(swaggerOptions.Host) ? null : swaggerOptions.Host),
                    basePath: (string.IsNullOrEmpty(swaggerOptions.Basepath) ? null : swaggerOptions.Basepath)
                    );
                var outputPath = string.IsNullOrEmpty(swaggerOptions.OutputPath)
                        ? null
                        : System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), swaggerOptions.OutputPath)
                        ;
                using (var streamWriter = (!string.IsNullOrEmpty(outputPath) ? System.IO.File.CreateText(outputPath) : System.Console.Out)) {
                    IOpenApiWriter writer;

                    if (swaggerOptions.Yaml) {
                        writer = new OpenApiYamlWriter(streamWriter);
                    } else {
                        writer = new OpenApiJsonWriter(streamWriter);
                    }

                    if (swaggerOptions.SerializeasV2) {
                        swagger.SerializeAsV2(writer);
                    } else {
                        swagger.SerializeAsV3(writer);
                    }

                    if (string.IsNullOrEmpty(outputPath)) {
                        //
                    } else { 
                        System.Console.Out.WriteLine($"Swagger {(swaggerOptions.SerializeasV2 ? "V2" : "V3")} {(swaggerOptions.Yaml ? "YAML" : "JSON")} successfully written to {outputPath}");
                    }
                }
                return true;
            } else {
                return false;
            }
        }
    }
}
