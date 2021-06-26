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
            //����ѡ��ģʽ��ȡ�����ļ�
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

            //��ǰ�û�
            #region ��ǰ�û���Ϣ
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
            //��׼�����ַ���
            string baseStrConn = Configuration.GetConnectionString("Default");
            //����freesql����

            IdleBus<IFreeSql> idleBus = new IdleBus<IFreeSql>(TimeSpan.FromMinutes(10));
            services.AddSingleton(idleBus);

            services.AddFreeSql(Configuration);
            #endregion

            //��չ���� ע��IdentityServer����JWT��֤
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
