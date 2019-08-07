using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Drypoint.Application.Authorization.Permissions
{
    /// <summary>
    /// This class is used to permissions for users.
    /// </summary>
    public interface IPermissionChecker
    {
        Task<bool> IsGrantedAsync(string permissionName);

        Task<bool> IsGrantedAsync(IUserIdentifier userIdentifier, string permissionName);
    }
}
