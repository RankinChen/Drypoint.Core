using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drypoint.Host.Core.Configuration;
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
            _logger.LogInformation($"IdentityServer:IsEnabled:{_appConfiguration["IdentityServer:IsEnabled"]}");
            //Identity server
            if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"]))
            {
                //IdentityServerRegistrar.Register(services, _appConfiguration);
            }

            //添加自定义API文档生成(支持文档配置)
            services.AddCustomSwaggerGen(_appConfiguration, _hostingEnvironment);
        }

        partial void CustomConfigure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //启用自定义API文档(支持文档配置)
            //app.UseCustomSwaggerUI(_appConfiguration);

            //启用中间件为生成的 Swagger 规范和 Swagger UI 提供服务
            app.UseCustomSwaggerUI(_appConfiguration);
        }
    }
}
