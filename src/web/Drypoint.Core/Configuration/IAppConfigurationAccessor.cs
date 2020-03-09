using System;
using System.Collections.Generic;
using System.Text;
using Drypoint.Unity;
using Drypoint.Unity.Dependency;
using Microsoft.Extensions.Configuration;

namespace Drypoint.Core.Configuration
{
    public interface IAppConfigurationAccessor:IScopedDependency
    {
        IConfigurationRoot Configuration { get; }
    }
}
