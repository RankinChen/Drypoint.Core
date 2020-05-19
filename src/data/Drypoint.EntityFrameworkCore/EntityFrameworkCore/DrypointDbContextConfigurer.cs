using Drypoint.Unity;
using Drypoint.Unity.EnumCollection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Drypoint.EntityFrameworkCore.EntityFrameworkCore
{
    public static class DrypointDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<DrypointDbContext> builder, string connectionString, DBCategoryEnum dbCategory = DBCategoryEnum.SQLServer)
        {
            switch (dbCategory)
            {
                case DBCategoryEnum.SQLServer:
                    builder.UseSqlServer(connectionString);
                    break;
                case DBCategoryEnum.PostgreSQL:
                    builder.UseNpgsql(connectionString);
                    break;
            }

        }

        public static void Configure(DbContextOptionsBuilder<DrypointDbContext> builder, DbConnection connection, DBCategoryEnum dbCategory)
        {
            switch (dbCategory)
            {
                case DBCategoryEnum.SQLServer:
                    builder.UseSqlServer(connection);
                    break;
                case DBCategoryEnum.PostgreSQL:
                    builder.UseNpgsql(connection);
                    break;
            }

        }
    }
}
