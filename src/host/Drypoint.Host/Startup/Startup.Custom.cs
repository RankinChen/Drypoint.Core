using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Drypoint.Host.Core.Authentication;
using Drypoint.Host.Core.Configuration;
using Drypoint.Host.Core.IdentityServer;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;

namespace Drypoint.Host.Startup
{
    public partial class Startup
    {
        /// <summary>
        /// 配置自定义服务
        /// </summary>
        /// <param name="services"></param>
        partial void ConfigureCustomServices(IServiceCollection services)
        {
            //授权相关:资源端代码
            IdentityModelEventSource.ShowPII = true;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            //访问客户端使用
            //.AddOpenIdConnect(ProtocolTypes.OpenIdConnect, "OpenID Connect", options =>
            //{
            //    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //    options.SignOutScheme = IdentityServerConstants.SignoutScheme;
            //    options.SaveTokens = true;
            //    //options.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(5);
            //    //options.TokenValidationParameters.RequireExpirationTime = true;
            //    options.Authority = configuration["IdentityServer:Authority"];
            //    options.ClientId = "hybrid";
            //})
            //资源端
            .AddIdentityServerAuthentication(options =>
            {
                //options.JwtValidationClockSkew = TimeSpan.Zero;
                options.Authority = _appConfiguration["IdentityServer:Authority"];
                options.ApiName = _appConfiguration["IdentityServer:ApiName"];
                options.ApiSecret = _appConfiguration["IdentityServer:ApiSecret"];
                options.RequireHttpsMetadata = false;
                //待测试
                //options.JwtBearerEvents = new JwtBearerEvents
                //{
                //    OnMessageReceived = QueryStringTokenResolver
                //};
            });

            //添加自定义API文档生成(支持文档配置)
            services.AddCustomSwaggerGen(_appConfiguration, _hostingEnvironment);
        }

        partial void CustomConfigure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //授权相关:资源端代码
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //启用中间件为生成的 Swagger 规范和 Swagger UI 提供服务
            app.UseCustomSwaggerUI(_appConfiguration);
        }
    }
}
