using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace Drypoint.SSO
{
    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel((context, opt) =>
                {
                    opt.AddServerHeader = false;
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    //根据环境变量加载不同的JSON配置
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                            optional: true, reloadOnChange: true);
                    //从环境变量添加配置
                    config.AddEnvironmentVariables();
                })
                //.UseIISIntegration()
                .ConfigureLogging((ILoggingBuilder logBuilder) =>
                {
                    logBuilder.AddNLog();
                    NLog.LogManager.LoadConfiguration("nlog.config");
                    //添加控制台日志,Docker环境下请务必启用
                    logBuilder.AddConsole();
                    //添加调试日志
                    logBuilder.AddDebug();
                })
                .UseStartup<Startup>();

        /*
        public static IHostBuilder CreateHostBuilder2(string[] args) =>
                Host.CreateDefaultBuilder(args)
                .ConfigureLogging((ILoggingBuilder logBuilder) =>
                {
                    logBuilder.AddNLog();
                    logBuilder.AddConsole();
                    //logBuilder.confi            
                    NLog.LogManager.LoadConfiguration("mynlog.config");
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:5012");
                    webBuilder.UseStartup<Startup>();
                });
        */
    }
}
