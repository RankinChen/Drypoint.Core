using System;
using System.Collections.Generic;
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
            //Identity server(注：服务端和资源端 应分开两个API)
            //授权相关：服务端代码
            services.AddIdentityServer(options =>
            {
                //用户交互配置 主要涉及到入口地址参数等
                options.UserInteraction = new IdentityServer4.Configuration.UserInteractionOptions
                {
                    LoginUrl = "/Account/Login",
                    LogoutUrl = "/Account/Logout",
                    ConsentUrl = "/Account/Consent",
                    //ErrorUrl = "/Account/Error",
                    LoginReturnUrlParameter = "ReturnUrl",
                    LogoutIdParameter = "logoutId",
                    ConsentReturnUrlParameter = "ReturnUrl",
                    ErrorIdParameter = "errorId",
                    CustomRedirectReturnUrlParameter = "ReturnUrl",
                    CookieMessageThreshold = 5
                };
            }).AddDeveloperSigningCredential()        //使用演示签名证书
            //.AddSigningCredential(new X509Certificate2(Path.Combine(AppContext.BaseDirectory, Configuration["Certs:Path"]), Configuration["Certs:Pwd"]))
              .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources(_appConfiguration))
              .AddInMemoryApiResources(IdentityServerConfig.GetApiResources(_appConfiguration))
              .AddInMemoryClients(IdentityServerConfig.GetClients(_appConfiguration))
              .AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>()
              .AddProfileService<CustomProfileService>();


            //授权相关:资源端代码
            AuthConfigurer.Configure(services, _appConfiguration);

            //添加自定义API文档生成(支持文档配置)
            services.AddCustomSwaggerGen(_appConfiguration, _hostingEnvironment);
        }

        partial void CustomConfigure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //授权相关：服务端代码
            app.UseIdentityServer();

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
