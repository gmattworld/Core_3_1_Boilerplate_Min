using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace APP.API.Utilities
{
    public static class SwaggerConfig
    {
        private static IEnumerable<string> Versions => new[] { "1.0", "2.0" };
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                Versions.ToList()
                    .ForEach(v =>
                        option.SwaggerDoc($"v{v}",
                            new OpenApiInfo
                            {
                                Title = $"APP API: v{v}",
                                Version = $"v{v}"
                            }));

                option.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                    var versions = methodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v.ToString()}" == docName);
                });

                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

                option.DocumentFilter<ReplaceVersionWithExactValueInPath>();
                option.OperationFilter<RemoveVersionFromParameter>();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var fullPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                option.IncludeXmlComments(fullPath);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                // Set the document expantion
                option.DocExpansion(DocExpansion.None);

                // Configure how the response is displayed
                option.DefaultModelRendering(ModelRendering.Example);

                option.DefaultModelExpandDepth(2);
                option.DefaultModelsExpandDepth(-1);
                option.DisplayOperationId();
                option.DisplayRequestDuration();
                option.EnableDeepLinking();
                option.EnableFilter();
                //option.MaxDisplayedTags(5);
                option.ShowExtensions();
                option.EnableValidator();
                option.InjectStylesheet("/swagger-ui/custom.css");
                //option.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Head);
                //option.IndexStream = () => GetType().Assembly.GetManifestResourceStream("CustomUIIndex.Swagger.index.html"); // requires file to be added as an embedded resource

                option.RoutePrefix = "swagger";
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(option.RoutePrefix) ? "." : "..";

                Versions.ToList()
                  .ForEach(v => option.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v{v}/swagger.json", $"APP API: v{v}"));
            });

            return app;
        }
    }

    public class RemoveVersionFromParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!operation.Parameters.Any())
                return;

            var versionParameter = operation.Parameters
                .FirstOrDefault(p => p.Name.ToLower() == "version");

            if (versionParameter != null)
                operation.Parameters.Remove(versionParameter);
        }
    }


    public class ReplaceVersionWithExactValueInPath : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (swaggerDoc == null)
                throw new ArgumentNullException(nameof(swaggerDoc));

            var replacements = new OpenApiPaths();

            foreach (var (key, value) in swaggerDoc.Paths)
            {
                replacements.Add(key.Replace("v{version}", swaggerDoc.Info.Version,
                        StringComparison.InvariantCulture), value);
            }

            swaggerDoc.Paths = replacements;
        }
    }

    public class ApiExplorerGroupPerVersionConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var controllerNamespace = controller.ControllerType.Namespace; // e.g. "Controllers.V1"
            var apiVersion = controllerNamespace.Split('.').Last().ToLower();

            controller.ApiExplorer.GroupName = apiVersion;
        }
    }

}
