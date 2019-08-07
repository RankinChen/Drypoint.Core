using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Application.Authorization.Permissions
{
    public interface IUserIdentifier
    {
        long UserId { get; }
        long? RoleId { get; }
    }
}
