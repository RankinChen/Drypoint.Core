using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Drypoint.Core.Common.Auditing;
using Drypoint.Core.Common;

namespace Drypoint.Core.Authorization.Roles
{
    [Table("DrypointRoleBase")]
    public class RoleBase : FullAuditedEntity<int>
    {
        [Required]
        [StringLength(32)]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(64)]
        public virtual string DisplayName { get; set; }

        public virtual bool IsStatic { get; set; }
        public virtual bool IsDefault { get; set; }

        [ForeignKey("RoleId")]
        public virtual ICollection<RolePermissionSetting> Permissions { get; set; }

        public RoleBase()
        {
            Name = Guid.NewGuid().ToString("N");
        }

        public RoleBase( string displayName)
            : this()
        {
            DisplayName = displayName;
        }

        public RoleBase( string name, string displayName)
            : this(displayName)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"[Role {Id}, Name={Name}]";
        }
    }
}
