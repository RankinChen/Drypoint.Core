using Drypoint.Model.Base;
using Drypoint.Unity.BaseServices;
using Drypoint.Unity.OptionsConfigModels;
using FreeSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Drypoint.Core.Extensions.BaseServices.StartupTask
{
    public class InitDBStartup : IStartupTask
    {
        private readonly IServiceProvider _serviceProvider;
        public InitDBStartup(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            using var scope = _serviceProvider.CreateScope(); ;
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var initDBTaskConfig = scope.ServiceProvider.GetRequiredService<IOptions<InitDBTaskConfig>>().Value;

            //创建数据库
            if (initDBTaskConfig.CreateDB)
            {
                await DbHelper.CreateDatabaseAsync(initDBTaskConfig);
            }

            var freeSqlBuilder = new FreeSqlBuilder()
                    .UseConnectionString(DataType.PostgreSQL, initDBTaskConfig.CreateDBConnectionString)
                    .UseAutoSyncStructure(false)
                    .UseLazyLoading(false)
                    .UseNoneCommandParameter(true);

            #region 监听所有命令
            //TODO 后续可改成配置
            #if DEBUG
            freeSqlBuilder.UseMonitorCommand(cmd => { }, (cmd, traceLog) =>
            {
                Console.WriteLine($"{cmd.CommandText}\r\n");
            });
#endif
            #endregion

            var fsql = freeSqlBuilder.Build();
            fsql.GlobalFilter.Apply<ISoftDelete>("SoftDelete", a => a.IsDeleted == false);

            //同步结构
            if (initDBTaskConfig.SyncStructure)
            {
                DbHelper.SyncStructure(fsql, initDBTaskConfig: initDBTaskConfig);
            }

            var currentUser = scope.ServiceProvider.GetRequiredService<ICurrentUser>();

            #region 审计数据
            fsql.Aop.AuditValue += (s, e) =>
            {
                DbHelper.AuditValue(e, currentUser);
            };
            #endregion 审计数据

            //同步数据
            if (initDBTaskConfig.SyncData)
            {
                await DbHelper.SyncDataAsync(fsql, initDBTaskConfig);
            }
        }
    }
}
