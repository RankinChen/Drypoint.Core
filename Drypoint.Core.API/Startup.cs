using Autofac;
using Drypoint.Core.Extensions;
using Drypoint.Core.Extensions.Authentication;
using Drypoint.Core.Extensions.AutoMappers;
using Drypoint.Core.Extensions.BaseServices;
using Drypoint.Core.Extensions.Configurations;
using Drypoint.Unity.BaseServices;
using Drypoint.Unity.OptionsConfigModels;
using FreeSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Yitter.IdGenerator;

namespace Drypoint.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static IFreeSql freeSql { get; private set; }

        readonly string LocalCorsPolicyName = "localhostCORS";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //配置选项模式读取配置文件
            services.AddCustomOptions(Configuration);

            //雪花漂移算法
            YitIdHelper.SetIdGenerator(new IdGeneratorOptions(1) { WorkerIdBitLength = 6 });

            var authManagement = Configuration.GetSection("Authentication").Get<AuthManagement>();

            services.AddControllers();

            //AutoMapper 
            #region AutoMapper
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            }, AppDomain.CurrentDomain.GetAssemblies());
            #endregion

            //当前用户
            #region 当前用户信息
            if (authManagement.IdentityServer.IsEnabled)
            {
                //is4
                services.TryAddSingleton<ICurrentUser, CurrentUserIdentiyServer>();
            }
            else
            {
                //jwt
                services.TryAddSingleton<ICurrentUser, CurrentUser>();
            }
            #endregion

            #region CORS
            services.AddCors(options =>
            {
                options.AddPolicy(LocalCorsPolicyName, policy =>
                {
                    policy
                        .WithOrigins(Configuration["CorUrls"].Split(",", StringSplitOptions.RemoveEmptyEntries).ToArray())
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials(); //不要和AllowAnyOrigin同时使用
                });
            });
            #endregion


            #region FreeSql
            services.AddFreeSql(Configuration);
            #endregion

            //扩展方法 注册IdentityServer或者JWT认证
            services.AuthConfigurer(Configuration);

            //Swagger
            services.AddCustomSwaggerGen(Configuration);

            //Cache
            services.AddCache(Configuration);

            //IP限流
            services.AddIpRateLimit(Configuration);

            //阻止Log接收状态消息 ???
            services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);
        }

        /// <summary>
        /// Autofac执行注入的地方 ConfigureServices之后执行
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacDIExtensionsModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //IP限流
            app.UseIpRateLimiting(Configuration);

            //CORS
            app.UseCors(LocalCorsPolicyName);

            //静态文件
            app.UseUploadConfig();

            app.UseHttpsRedirection();

            //路由
            app.UseRouting();

            //认证
            app.UseAuthentication();

            //授权
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Swagger Api文档
            app.UseCustomSwaggerUI(Configuration);
        }
    }
}
