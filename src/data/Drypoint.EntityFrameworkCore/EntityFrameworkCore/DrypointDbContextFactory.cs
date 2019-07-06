using System;
using System.Collections.Generic;
using System.Text;
using Drypoint.Core;
using Drypoint.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Drypoint.EntityFrameworkCore.EntityFrameworkCore
{
    public class DrypointDbContextFactory : IDesignTimeDbContextFactory<DrypointDbContext>
    {
        public DrypointDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DrypointDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);

            DrypointDbContextConfigurer.Configure(builder, configuration.GetConnectionString(DrypointConsts.ConnectionStringName));

            return new DrypointDbContext(builder.Options);
        }
    }
}
