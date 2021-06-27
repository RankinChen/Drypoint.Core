using Drypoint.Unity.BaseServices;
using Drypoint.Unity.Cache;
using Drypoint.Unity.Enums;
using Drypoint.Unity.OptionsConfigModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drypoint.Core.Extensions
{
    public static class CacheExtensions
    {
        public static void AddCache(this IServiceCollection services, IConfiguration configuration)
        {
            var cacheConfig = configuration.GetSection("CacheConfig").Get<CacheConfig>();

            if (cacheConfig.Type == CacheType.Memory)
            {
                services.AddMemoryCache();
                services.AddSingleton<ICache, MemoryCache>();
            }
            else
            {
                var csredis = new CSRedis.CSRedisClient(cacheConfig.Redis.ConnectionString);
                RedisHelper.Initialization(csredis);
                services.AddSingleton<ICache, RedisCache>();
            }
        }
    }
}
