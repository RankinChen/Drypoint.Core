using AspNetCoreRateLimit;
using Drypoint.Unity.Enums;
using Drypoint.Unity.OptionsConfigModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drypoint.Core.Extensions
{
    public static class RateLimitServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Ip限流
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddIpRateLimit(this IServiceCollection services, IConfiguration configuration)
        {
            var cacheConfig = configuration.GetSection("CacheConfig").Get<CacheConfig>();

            #region IP限流
            if (configuration.GetSection("RateLimit").GetValue<bool>(key: "IsEnabled", defaultValue: false))
            {
                services.Configure<IpRateLimitOptions>(configuration.GetSection("RateLimit:IpRateLimiting"));
                services.Configure<IpRateLimitPolicies>(configuration.GetSection("RateLimit:IpRateLimitPolicies"));

                if (cacheConfig.TypeRateLimit == CacheType.Memory)
                {
                    //内存
                    services.AddMemoryCache();
                    services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
                    services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
                }
                else
                {
                    //redis
                    var redisRateLimit = new CSRedis.CSRedisClient(cacheConfig.Redis.ConnectionStringRateLimit);
                    services.AddSingleton<IDistributedCache>(new CSRedisCache(redisRateLimit));
                    services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
                    services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();
                }
                services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            }
            #endregion IP限流
        }

        public static void UseIpRateLimiting(this IApplicationBuilder app, IConfiguration configuration)
        {
            var cacheConfig = configuration.GetSection("CacheConfig").Get<CacheConfig>();
            if (configuration.GetSection("RateLimit").GetValue<bool>(key: "IsEnabled", defaultValue: false))
            {
                app.UseIpRateLimiting();
            }
        }
    }
}
