using Drypoint.Unity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Host.Core.Configuration
{
    public static class SwaggerDocExtensions
    {
        public static void AddCustomSwaggerGen(this IServiceCollection services, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            //以下二选一
            //注册OpenAPI
            //services.AddOpenApiDocument();

            //注册Swagger
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = configuration["SwaggerDoc:Version"];
                    document.Info.Title = configuration["SwaggerDoc:Title"];
                    document.Info.Description = configuration["SwaggerDoc:Description"];
                    document.Info.TermsOfService = configuration["SwaggerDoc:TermsOfService"];
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = configuration["SwaggerDoc:Contact:Name"],
                        Email = configuration["SwaggerDoc:Contact:Email"],
                        Url = configuration["SwaggerDoc:Contact:Url"]
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = configuration["SwaggerDoc:License:Name"],
                        Url = configuration["SwaggerDoc:License:Url"]
                    };
                };
                config.DocumentName = "App";
                config.ApiGroupNames = new[] { DrypointConsts.AppAPIGroupName };
            })
            .AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = configuration["SwaggerDoc:Version"];
                    document.Info.Title = configuration["SwaggerDoc:Title"];
                    document.Info.Description = configuration["SwaggerDoc:Description"];
                    document.Info.TermsOfService = configuration["SwaggerDoc:TermsOfService"];
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = configuration["SwaggerDoc:Contact:Name"],
                        Email = configuration["SwaggerDoc:Contact:Email"],
                        Url = configuration["SwaggerDoc:Contact:Url"]
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = configuration["SwaggerDoc:License:Name"],
                        Url = configuration["SwaggerDoc:License:Url"]
                    };
                };
                config.DocumentName = "Admin";
                config.ApiGroupNames = new[] { DrypointConsts.AdminAPIGroupName };
            });
        }

        /// <summary>
        /// 启用自定义API文档
        /// </summary>
        public static void UseCustomSwaggerUI(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseOpenApi();
            //以下两种 二选一
            app.UseSwaggerUi3(config =>
             {

             });
            //app.UseReDoc();
        }
    }
}
