using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Drypoint.Core.Configuration;
using Microsoft.AspNetCore.Http;
using Drypoint.Core.IdentityServer;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;

namespace Drypoint.SSO
{
    public partial class Startup
    {
        private readonly ILogger _logger;
        private const string LocalCorsPolicyName = "localhost";

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment, ILogger<Startup> logger)
        {
            Environment = environment;
            Configuration = configuration;
            _logger = logger;
            _logger.LogInformation($"运行环境:{Environment.EnvironmentName}");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //DI
            services.AddServiceRegister();

            //MVC
            services.AddMvc(option => option.EnableEndpointRouting = false).AddNewtonsoftJson();
            services.AddMvc(options =>
            {
                //options.Filters.Add(new CorsAuthorizationFilterFactory(LocalCorsPolicyName));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //Configure CORS for APP
            services.AddCors(options =>
            {
                options.AddPolicy(LocalCorsPolicyName, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            //设置https重定向端口
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 443;
            });


            //授权相关：服务端代码
            services.AddIdentityServer(options =>
            {
                ////用户交互配置 主要涉及到入口地址参数等
                //options.UserInteraction = new IdentityServer4.Configuration.UserInteractionOptions
                //{
                //    LoginUrl = "/Account/Login",
                //    LogoutUrl = "/Account/Logout",
                //    ConsentUrl = "/Account/Consent",
                //    //ErrorUrl = "/Account/Error",
                //    LoginReturnUrlParameter = "ReturnUrl",
                //    LogoutIdParameter = "logoutId",
                //    ConsentReturnUrlParameter = "ReturnUrl",
                //    ErrorIdParameter = "errorId",
                //    CustomRedirectReturnUrlParameter = "ReturnUrl",
                //    CookieMessageThreshold = 5
                //};
            }).AddDeveloperSigningCredential()        //使用演示签名证书
            //.AddSigningCredential(new X509Certificate2(Path.Combine(AppContext.BaseDirectory, Configuration["Certs:Path"]), Configuration["Certs:Pwd"]))
              .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources(Configuration))
              .AddInMemoryApiResources(IdentityServerConfig.GetApiResources(Configuration))
              .AddInMemoryClients(IdentityServerConfig.GetClients(Configuration))
              //使用内存测试数据身份认证 TODO
              .AddTestUsers(IdentityServerConfig.GetTestUser())
              //添加自定义claim
              .AddProfileService<ProfileService>()
              //自定义用户密码验证模式
              .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();

            /*
            services.AddAuthentication()
                //开启google登陆
                .AddGoogle(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    //register your IdentityServer with Google at https://console.developers.google.com
                    //enable the Google + API
                    //set the redirect URI to http://localhost:5000/signin-google
                    options.ClientId = "xxxxxxxxxxxxxxxxxm";
                    options.ClientSecret = "xxxxxxxxxxxxxxxxx";
                });
            */
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "text/html";
                        var ex = context.Features.Get<IExceptionHandlerFeature>();
                        if (ex != null)
                        {
                            var err = $"<h1>Error: {ex.Error.Message}</h1>{ex.Error.StackTrace}";
                            context.Response.Headers.Add("application-error", ex.Error.Message);
                            context.Response.Headers.Add("access-control-expose-headers", "application-error");
                            context.Response.Headers.Add("access-control-allow-origin", "*");
                            await context.Response.WriteAsync(err).ConfigureAwait(false);
                        }
                    });
                });// this will add the global exception handle for production evironment.
            }

            app.UseCors(LocalCorsPolicyName); //Enable CORS!

            //开启HTTPS重定向
            app.UseHttpsRedirection();


            //授权相关：服务端代码
            app.UseIdentityServer();

            app.UseStaticFiles();

            //app.UseMvcWithDefaultRoute();
            app.UseMvc();
        }
    }
}
