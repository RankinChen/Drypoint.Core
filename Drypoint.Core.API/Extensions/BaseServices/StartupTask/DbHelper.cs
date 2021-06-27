using Drypoint.Model.Base;
using Drypoint.Model.Base.Auditing;
using Drypoint.Model.Models.Admin;
using Drypoint.Unity;
using Drypoint.Unity.Attributes;
using Drypoint.Unity.BaseServices;
using Drypoint.Unity.Extensions;
using Drypoint.Unity.Helpers;
using Drypoint.Unity.OptionsConfigModels;
using FreeSql;
using FreeSql.Aop;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Yitter.IdGenerator;

namespace Drypoint.Core.Extensions.BaseServices.StartupTask
{
    public class DbHelper
    {
        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="initDBTaskConfig"></param>
        /// <returns></returns>
        public async static Task CreateDatabaseAsync(InitDBTaskConfig initDBTaskConfig)
        {
            if (!initDBTaskConfig.CreateDB)
            {
                return;
            }

            var db = new FreeSqlBuilder()
                    .UseConnectionString(DataType.PostgreSQL, initDBTaskConfig.CreateDBConnectionString)
                    .Build();

            try
            {
                Console.WriteLine("\r\n create database started");
                await db.Ado.ExecuteNonQueryAsync(initDBTaskConfig.CreateDBSql);
                Console.WriteLine(" create database succeed");
            }
            catch (Exception e)
            {
                Console.WriteLine($" create database failed.\n {e.Message}");
            }
        }

        /// <summary>
        /// 获得指定程序集表实体
        /// </summary>
        /// <returns></returns>
        public static Type[] GetEntityTypes()
        {
            List<string> assemblyNames = new List<string>()
            {
                "Drypoint.Model.Models"
            };

            List<Type> entityTypes = new List<Type>();

            foreach (var assemblyName in assemblyNames)
            {
                foreach (Type type in Assembly.Load(assemblyName).GetExportedTypes())
                {
                    foreach (Attribute attribute in type.GetCustomAttributes())
                    {
                        if (attribute is TableAttribute tableAttribute)
                        {
                            if (tableAttribute.DisableSyncStructure == false)
                            {
                                entityTypes.Add(type);
                            }
                        }
                    }
                }
            }

            return entityTypes.ToArray();
        }

        /// <summary>
        /// 同步结构
        /// </summary>
        public static void SyncStructure(IFreeSql db, string msg = null, InitDBTaskConfig initDBTaskConfig = null)
        {
            //打印结构比对脚本
            //var dDL = db.CodeFirst.GetComparisonDDLStatements<PermissionEntity>();
            //Console.WriteLine("\r\n " + dDL);

            //打印结构同步脚本
            //db.Aop.SyncStructureAfter += (s, e) =>
            //{
            //    if (e.Sql.NotNull())
            //    {
            //        Console.WriteLine(" sync structure sql:\n" + e.Sql);
            //    }
            //};

            // 同步结构
            var dbType = DataType.PostgreSQL;
            Console.WriteLine($"\r\n {(msg.NotNull() ? msg : $"sync {dbType} structure")} started");

            //获得指定程序集表实体
            var entityTypes = GetEntityTypes();

            db.CodeFirst.SyncStructure(entityTypes);
            Console.WriteLine($" {(msg.NotNull() ? msg : $"sync {dbType} structure")} succeed");
        }

        /// <summary>
        /// 审计数据
        /// </summary>
        /// <param name="e"></param>
        /// <param name="user"></param>
        public static void AuditValue(AuditValueEventArgs e, ICurrentUser user)
        {
            if (e.Property.GetCustomAttribute<ServerTimeAttribute>(false) != null
                   && (e.Column.CsType == typeof(DateTime) || e.Column.CsType == typeof(DateTime?))
                   && (e.Value == null || (DateTime)e.Value == default || (DateTime?)e.Value == default))
            {
                e.Value = DateTime.Now;
            }

            if (e.Column.CsType == typeof(long)
            && e.Property.GetCustomAttribute<SnowflakeAttribute>(false) is SnowflakeAttribute snowflakeAttribute
            && (e.Value == null || (long)e.Value == default || (long?)e.Value == default))
            {
                e.Value = YitIdHelper.NextId();
            }

            if (user == null || user.Id <= 0)
            {
                return;
            }

            if (e.AuditValueType == AuditValueType.Insert)
            {
                switch (e.Property.Name)
                {
                    case nameof(IAudited<long>.CreatorUserId):
                        if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                        {
                            e.Value = user.Id;
                        }
                        break;
                    case nameof(IAudited<long>.CreationTime):
                        if (e.Value == null || ((string)e.Value).IsNull())
                        {
                            e.Value = DateTime.Now;
                        }
                        break;
                    case nameof(ITenant.TenantId):
                        if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                        {
                            e.Value = user.TenantId;
                        }
                        break;
                }
            }
            else if (e.AuditValueType == AuditValueType.Update)
            {
                switch (e.Property.Name)
                {
                    case nameof(IAudited<long>.ModifierUserId):
                        e.Value = user.Id;
                        break;

                    case nameof(IAudited<long>.ModificationTime):
                        e.Value = DateTime.Now;
                        break;
                }
            }
        }
        
        /// <summary>
        /// 同步数据
        /// </summary>
        /// <returns></returns>
        public static async Task SyncDataAsync(IFreeSql db, InitDBTaskConfig initDBTaskConfig)
        {
            try
            {
                //db.Aop.CurdBefore += (s, e) =>
                //{
                //    Console.WriteLine($"{e.Sql}\r\n");
                //};

                Console.WriteLine("\r\n sync data started");

                db.Aop.AuditValue += SyncDataAuditValue;

                var filePath = Path.Combine(AppContext.BaseDirectory, $"Db/Data/data-share.json").ToPath();
                var jsonData = FileHelper.ReadFile(filePath);
                var data = System.Text.Json.JsonSerializer.Deserialize<InitData>(jsonData);

                using (var uow = db.CreateUnitOfWork())
                using (var tran = uow.GetOrBeginTransaction())
                {
                    var dualRepo = db.GetRepository<DualEntity>();
                    dualRepo.UnitOfWork = uow;
                    if (!await dualRepo.Select.AnyAsync())
                    {
                        await dualRepo.InsertAsync(new DualEntity { });
                    }

                    //admin
                    //await InitDtDataAsync(db, uow, tran, data.Dictionaries, dbConfig);
                    //await InitDtDataAsync(db, uow, tran, data.ApiTree, dbConfig);
                    //await InitDtDataAsync(db, uow, tran, data.ViewTree, dbConfig);
                    //await InitDtDataAsync(db, uow, tran, data.PermissionTree, dbConfig);
                    //await InitDtDataAsync(db, uow, tran, data.Users, dbConfig);
                    //await InitDtDataAsync(db, uow, tran, data.Roles, dbConfig);
                    //await InitDtDataAsync(db, uow, tran, data.UserRoles, dbConfig);
                    //await InitDtDataAsync(db, uow, tran, data.RolePermissions, dbConfig);
                    //await InitDtDataAsync(db, uow, tran, data.Tenants, dbConfig);
                    //await InitDtDataAsync(db, uow, tran, data.TenantPermissions, dbConfig);
                    //await InitDtDataAsync(db, uow, tran, data.PermissionApis, dbConfig);

                    //人事
                    //await InitDtDataAsync(db, uow, tran, data.OrganizationTree, dbConfig);

                    uow.Commit();
                }

                db.Aop.AuditValue -= SyncDataAuditValue;

                Console.WriteLine(" sync data succeed\r\n");
            }
            catch (Exception ex)
            {
                throw new Exception($" sync data failed.\n{ex.Message}");
            }
        }


        /// <summary>
        /// 同步数据审计方法
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private static void SyncDataAuditValue(object s, AuditValueEventArgs e)
        {
            var user = new { Id = 161223411986501, Name = "admin", TenantId = 161223412138053 };

            if (e.Property.GetCustomAttribute<ServerTimeAttribute>(false) != null
                   && (e.Column.CsType == typeof(DateTime) || e.Column.CsType == typeof(DateTime?))
                   && (e.Value == null || (DateTime)e.Value == default || (DateTime?)e.Value == default))
            {
                e.Value = DateTime.Now;
            }

            if (e.Column.CsType == typeof(long)
            && e.Property.GetCustomAttribute<SnowflakeAttribute>(false) != null
            && (e.Value == null || (long)e.Value == default || (long?)e.Value == default))
            {
                e.Value = YitIdHelper.NextId();
            }

            if (user == null || user.Id <= 0)
            {
                return;
            }

            if (e.AuditValueType == AuditValueType.Insert)
            {
                switch (e.Property.Name)
                {
                    case nameof(IAudited<long>.CreatorUserId):
                        if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                        {
                            e.Value = user.Id;
                        }
                        break;
                    case nameof(IAudited<long>.CreationTime):
                        if (e.Value == null || ((string)e.Value).IsNull())
                        {
                            e.Value = DateTime.Now;
                        }
                        break;
                    case nameof(ITenant.TenantId):
                        if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                        {
                            e.Value = user.TenantId;
                        }
                        break;
                }
            }
            else if (e.AuditValueType == AuditValueType.Update)
            {
                switch (e.Property.Name)
                {
                    case nameof(IAudited<long>.ModifierUserId):
                        e.Value = user.Id;
                        break;

                    case nameof(IAudited<long>.ModificationTime):
                        e.Value = DateTime.Now;
                        break;
                }
            }
        }

        
        /// <summary>
        /// 初始化数据表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="tran"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static async Task InitDtDataAsync<T>(
            IFreeSql db,
            IUnitOfWork unitOfWork,
            System.Data.Common.DbTransaction tran,
            T[] data
        ) where T : class
        {
            var table = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() as TableAttribute;
            var tableName = table.Name;

            try
            {
                if (await db.Queryable<T>().AnyAsync())
                {
                    Console.WriteLine($" table: {tableName} record already exists");
                    return;
                }

                if (!(data?.Length > 0))
                {
                    Console.WriteLine($" table: {tableName} import data []");
                    return;
                }

                var repo = db.GetRepository<T>();
                var insert = db.Insert<T>();
                if (unitOfWork != null)
                {
                    repo.UnitOfWork = unitOfWork;
                    insert = insert.WithTransaction(tran);
                }

                var isIdentity = CheckIdentity<T>();
                if (isIdentity)
                {
                    await insert.AppendData(data).InsertIdentity().ExecuteAffrowsAsync();
                }
                else
                {
                    repo.DbContextOptions.EnableAddOrUpdateNavigateList = true;
                    await repo.InsertAsync(data);
                }

                Console.WriteLine($" table: {tableName} sync data succeed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" table: {tableName} sync data failed.\n{ex.Message}");
            }
        }

        /// <summary>
        /// 检查实体属性是否为自增长
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static bool CheckIdentity<T>() where T : class
        {
            var isIdentity = false;
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (property.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault() is ColumnAttribute columnAttribute && columnAttribute.IsIdentity)
                {
                    isIdentity = true;
                    break;
                }
            }

            return isIdentity;
        }
    }

    /// <summary>
    /// 数据 TODO
    /// </summary>
    public class InitData
    {
    }

}
