using Drypoint.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Drypoint.Unity.Dependency;

namespace Drypoint.Core.Configuration
{
    public class AppConfigurationAccessor : IAppConfigurationAccessor
    {
        public IConfigurationRoot Configuration { get; }

        public AppConfigurationAccessor(IHostingEnvironment env)
        {
            Configuration = env.GetAppConfiguration();
        }
    }
}
