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
            //����ѡ��ģʽ��ȡ�����ļ�
            services.AddCustomOptions(Configuration);

            //ѩ��Ư���㷨
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

            //��ǰ�û�
            #region ��ǰ�û���Ϣ
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
                        .AllowCredentials(); //��Ҫ��AllowAnyOriginͬʱʹ��
                });
            });
            #endregion


            #region FreeSql
            services.AddFreeSql(Configuration);
            #endregion

            //��չ���� ע��IdentityServer����JWT��֤
            services.AuthConfigurer(Configuration);

            //Swagger
            services.AddCustomSwaggerGen(Configuration);

            //Cache
            services.AddCache(Configuration);

            //IP����
            services.AddIpRateLimit(Configuration);

            //��ֹLog����״̬��Ϣ ???
            services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);
        }

        /// <summary>
        /// Autofacִ��ע��ĵط� ConfigureServices֮��ִ��
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

            //IP����
            app.UseIpRateLimiting(Configuration);

            //CORS
            app.UseCors(LocalCorsPolicyName);

            //��̬�ļ�
            app.UseUploadConfig();

            app.UseHttpsRedirection();

            //·��
            app.UseRouting();

            //��֤
            app.UseAuthentication();

            //��Ȩ
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Swagger Api�ĵ�
            app.UseCustomSwaggerUI(Configuration);
        }
    }
}
