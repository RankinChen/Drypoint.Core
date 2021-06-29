using Drypoint.Unity.OptionsConfigModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drypoint.Core.Extensions.Configurations
{
    public static class OptionsConfig
    {
        /// <summary>
        /// 使用选项模式绑定分层配置数据
        /// 参考 https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1#default-configuration
        /// 关于IOptions
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthManagement>(configuration.GetSection("Authentication"));
            services.Configure<SwaggerDocConfig>(configuration.GetSection("SwaggerDoc"));
            services.Configure<UploadConfig>(configuration.GetSection("FileUploadConfig"));
            services.Configure<CacheConfig>(configuration.GetSection("CacheConfig"));
            services.Configure<InitDBTaskConfig>(configuration.GetSection("InitDBTaskConfig"));
        }
    }
}
