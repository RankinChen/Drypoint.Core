using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Drypoint.EntityFrameworkCore.EntityFrameworkCore;

namespace Drypoint.Host.Core.IdentityServer
{
    public static class IdentityServerRegistrar
    {
        public static void Register(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()        //使用演示签名证书
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                .AddInMemoryClients(IdentityServerConfig.GetClients(configuration));
        }
    }
}
