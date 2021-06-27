using Drypoint.Unity;
using Drypoint.Unity.OptionsConfigModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Drypoint.Core.Extensions
{
    public static class SwaggerDocExtensions
    {
        public static void AddCustomSwaggerGen(this IServiceCollection services, IConfiguration configuration)
        {
            var authManagement = configuration.GetSection("Authentication").Get<AuthManagement>();
            if (authManagement.IdentityServer.IsEnabled)
            {
                IdentityServerSecurity(services, configuration);
            }
            else
            {
                JwtBearerSecurity(services, configuration);
            }
        }

        /// <summary>
        /// IdentityServer4 使用
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void IdentityServerSecurity(IServiceCollection services, IConfiguration configuration)
        {
            var authManagement = configuration.GetSection("Authentication").Get<AuthManagement>();
            var openApiInfo = GenOpenAPIInfo(configuration);
            //注册Swagger
            foreach (var groupName in DrypointConsts.ApiGroups)
            {
                services.AddSwaggerDocument(config =>
                {
                    config.AddSecurity("Bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme()
                    {
                        Type = OpenApiSecuritySchemeType.OAuth2,
                        Name = "授权",
                        Description = "尝试获取授权",
                        Flows = new OpenApiOAuthFlows()
                        {
                            AuthorizationCode = new OpenApiOAuthFlow()
                            {
                                Scopes = new Dictionary<string, string>
                                {
                                { "Drypoint_Host_API", "主要API" },
                                { "openid", "openid" },
                                { "profile", "profile" },
                                { "email", "Email" },
                                { "phone", "Phone" },
                                },
                                AuthorizationUrl = $"{authManagement.IdentityServer.Authority}/connect/authorize",
                                TokenUrl = $"{authManagement.IdentityServer.Authority}/connect/token"
                            },
                        }
                    });
                    config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
                    config.PostProcess = document => document.Info = openApiInfo;
                    config.DocumentName = $"{groupName} Document";
                    config.ApiGroupNames = new[] { groupName };
                });
            }
        }

        /// <summary>
        /// JwtBearer 使用
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void JwtBearerSecurity(IServiceCollection services, IConfiguration configuration)
        {
            var openApiInfo = GenOpenAPIInfo(configuration);

            //注册Swagger
            foreach (var groupName in DrypointConsts.ApiGroups)
            {
                services.AddSwaggerDocument(config =>
                {
                    config.AddSecurity("Bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme()
                    {
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Name = "授权",
                        Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）",
                        In = OpenApiSecurityApiKeyLocation.Header,
                    });
                    config.PostProcess = document => document.Info = openApiInfo;
                    config.DocumentName = $"{groupName} Document";
                    config.ApiGroupNames = new[] { groupName };
                });
            }
        }

        private static OpenApiInfo GenOpenAPIInfo(IConfiguration configuration)
        {
            var swaggerDocConfig = configuration.GetSection("SwaggerDoc").Get<SwaggerDocConfig>();

            var openApiInfo = new OpenApiInfo()
            {
                Version = swaggerDocConfig.Version,
                Title = swaggerDocConfig.Title,
                Description = swaggerDocConfig.Description,
                TermsOfService = swaggerDocConfig.TermsOfService,
                Contact = new OpenApiContact
                {
                    Name = swaggerDocConfig.Contact.Name,
                    Email = swaggerDocConfig.Contact.Email,
                    Url = swaggerDocConfig.Contact.Url
                },
                License = new OpenApiLicense
                {
                    Name = swaggerDocConfig.License.Name,
                    Url = swaggerDocConfig.License.Url
                }
            };
            return openApiInfo;
        }



        /// <summary>
        /// 启用自定义API文档
        /// </summary>
        public static void UseCustomSwaggerUI(this IApplicationBuilder app, IConfiguration configuration)
        {
            var swaggerDocConfig = configuration.GetSection("SwaggerDoc").Get<SwaggerDocConfig>();
            app.UseOpenApi();
            app.UseSwaggerUi3(config =>
            {
                if (swaggerDocConfig.Authorize.IsShow)
                {
                    config.OAuth2Client = new OAuth2ClientSettings
                    {
                        ClientId = swaggerDocConfig.Authorize.Client.ClientId,
                        ClientSecret = swaggerDocConfig.Authorize.Client.ClientSecret,
                        AppName = "API 端测试授权",

                        //在授权认证的时候地址栏后面添加请求参数 
                        /*
                        AdditionalQueryStringParameters = {
                            {"response_type","code id_token" },
                            { "redirect_uri","http://localhost:7000/signin-oidc"}
                        }
                        */
                    };
                }
            });
        }
    }
}
