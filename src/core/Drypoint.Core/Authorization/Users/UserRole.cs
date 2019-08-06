using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Drypoint.Core.Common.Auditing;
using Drypoint.Core.Authorization.Roles;

namespace Drypoint.Core.Authorization.Users
{
    [Table("DrypointUserRoles")]
    public class UserRole : CreationAuditedEntity<long>
    {
        public virtual long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual int RoleId { get; set; }


        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        public UserRole()
        {

        }
        public UserRole(long userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}
