using Drypoint.Unity.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Drypoint.Application.Authorization.Permissions
{
    public class PermissionChecker : IPermissionChecker, ITransientDependency
    {
        public Task<bool> IsGrantedAsync(string permissionName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsGrantedAsync(IUserIdentifier userIdentifier, string permissionName)
        {
            throw new NotImplementedException();
        }
    }
}
