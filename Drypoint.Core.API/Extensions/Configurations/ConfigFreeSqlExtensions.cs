using Drypoint.Unity;
using Drypoint.Unity.Enums;
using FreeSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drypoint.Core.Extensions.Configurations
{
    public static class ConfigFreeSqlExtensions
    {
        public static void AddFreeSql(this IServiceCollection services, IConfiguration configuration)
        {
            IdleBus<DbNameType,IFreeSql> idleBus = new IdleBus<DbNameType,IFreeSql>(TimeSpan.FromMinutes(10));

            idleBus.Register(DbNameType.DrypointCoreDB, () => CreateFreeSql(configuration.GetConnectionString(DrypointConsts.Default_ConnectionStringName)));
            idleBus.Register(DbNameType.DrypointIdsDB, () => CreateFreeSql(configuration.GetConnectionString(DrypointConsts.IdsDB_ConnectionStringName)));

            services.AddSingleton(idleBus);
        }

        /// <summary>
        /// 创建IFreeSql实例
        /// </summary>
        private static IFreeSql CreateFreeSql(string connectionString)
        {
            return new FreeSqlBuilder()
            .UseConnectionString(DataType.PostgreSQL, connectionString)
            .UseAutoSyncStructure(false)
            .UseLazyLoading(false)
            .UseNoneCommandParameter(true)
            .Build();
        }

    }

}
