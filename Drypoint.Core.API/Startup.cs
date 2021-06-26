using Drypoint.Core.Extensions.Authentication;
using Drypoint.Core.Extensions.Authentication.Services;
using Drypoint.Core.Extensions.AutoMappers;
using Drypoint.Core.Extensions.Configurations;
using Drypoint.Unity.Auth;
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
using System.Linq;
using System.Threading.Tasks;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {            
            //配置选项模式读取配置文件
            services.AddCustomOptions(Configuration);

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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
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

            #region FreeSql
            //标准连接字符串
            string baseStrConn = Configuration.GetConnectionString("Default");
            //构建freesql对象

            IdleBus<IFreeSql> idleBus = new IdleBus<IFreeSql>(TimeSpan.FromMinutes(10));
            services.AddSingleton(idleBus);

            services.AddFreeSql(Configuration);
            #endregion

            //扩展方法 注册IdentityServer或者JWT认证
            AuthConfigurer.Configure(services, Configuration);

            //Swagger
            services.AddCustomSwaggerGen(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCustomSwaggerUI(Configuration);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
