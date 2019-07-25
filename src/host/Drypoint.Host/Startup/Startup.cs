using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Drypoint.Host.Core.Configuration;
using Drypoint.Extensions;
using Microsoft.AspNetCore.Http;
using NLog.Extensions.Logging;
using Drypoint.Host.Core.IdentityServer;
using Newtonsoft.Json;

namespace Drypoint.Host.Startup
{
    public partial class Startup
    {
        private readonly ILogger _logger;
        private const string LocalCorsPolicyName = "localhost";

        private readonly IConfigurationRoot _appConfiguration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IHostingEnvironment env, ILogger<Startup> logger)
        {
            _hostingEnvironment = env;
            _appConfiguration = env.GetAppConfiguration();
            _logger = logger;
            _logger.LogInformation($"运行环境:{env.EnvironmentName}");
        }

        /// <summary>
        /// 配置自定义服务
        /// </summary>
        /// <param name="services"></param>
        partial void ConfigureCustomServices(IServiceCollection services);

        partial void CustomConfigure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory);

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //DI
            services.AddServiceRegister();

            //初始化缓存
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = _appConfiguration["RedisConnectionString"];
            });

            //MVC
            services.AddMvc(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory(LocalCorsPolicyName));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //不使用驼峰样式的key,按照Model中的属性名进行命名
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });


            var sbuilder = services.AddSignalR(options => { options.EnableDetailedErrors = true; });

            if (!_appConfiguration["RedisConnectionString"].IsNullOrWhiteSpace())
            {
                _logger.LogWarning("RedisConnectionString:" + _appConfiguration["RedisConnectionString"]);
                sbuilder.AddRedis(_appConfiguration["RedisConnectionString"]);
            }

            //Configure CORS for APP
            services.AddCors(options =>
            {
                options.AddPolicy(LocalCorsPolicyName, builder =>
                {
                    builder
                        //.WithOrigins(
                        //    // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                        //    _appConfiguration["App:CorsOrigins"]
                        //        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        //        .Select(o => o.RemovePostFix("/"))
                        //        .ToArray()
                        //)
                        //.SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });            
            
            //Identity server
            if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"]))
            {
                IdentityServerRegistrar.Register(services, _appConfiguration);
            }
            services.AddAuthentication().AddAuthenConfigure(_appConfiguration);

            if (bool.Parse(_appConfiguration["App:HttpsRedirection"] ?? "false"))
            {
                //建议开启，以在浏览器显示安全图标
                //设置https重定向端口
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                    options.HttpsPort = 443;
                });
            }

            //是否启用HTTP严格传输安全协议(HSTS)
            if (bool.Parse(_appConfiguration["App:UseHsts"] ?? "false"))
            {
                services.AddHsts(options =>
                {
                    options.Preload = true;
                    options.IncludeSubDomains = true;
                    options.MaxAge = TimeSpan.FromDays(60);
                    options.ExcludedHosts.Add("example.com");
                });
            }

            try
            {
                _logger.LogWarning("ConfigureCustomServices  Begin...");
                ConfigureCustomServices(services);
                _logger.LogWarning("ConfigureCustomServices  End...");
            }
            catch (Exception ex)
            {
                _logger.LogError("执行ConfigureCustomServices出现错误", ex);
            }

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                if (bool.Parse(_appConfiguration["App:UseHsts"] ?? "false"))
                {
                    app.UseHsts();
                }
            }


            if (_appConfiguration["Logging:LogType"].ToLower()=="nlog")
            {
                loggerFactory.AddNLog();
            }
            
            app.UseCors(LocalCorsPolicyName); //Enable CORS!
            if (bool.Parse(_appConfiguration["App:HttpsRedirection"] ?? "false"))
            {
                _logger.LogWarning("准备启用HTTS跳转...");
                //建议开启，以在浏览器显示安全图标
                app.UseHttpsRedirection();
            }

            app.UseAuthentication();
            if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"]))
            {
                //使用路径http://xxxxxx/.well-known/openid-configuration查看
                app.UseIdentityServer();
            }
            
            app.UseStaticFiles();

            app.UseSignalR(routes =>
            {
                //routes.MapHub<ChatHub>("/signalr-chat");
            });

            try
            {
                _logger.LogWarning("应用自定义配置...");
                CustomConfigure(app, env, loggerFactory);
            }
            catch (Exception ex)
            {
                _logger.LogError("应用自定义配置出现错误", ex);
            }
        }
    }
}
