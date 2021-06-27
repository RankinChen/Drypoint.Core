using Drypoint.Model.Base;
using Drypoint.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Model.Models
{
    /// <summary>
    /// 角色跟权限关联表
    /// </summary>
    public class RoleModulePermission:EntityFull<long>
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public virtual Role Role { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public long ModuleId { get; set; }

        /// <summary>
        /// 菜单
        /// </summary>
        public virtual Module Module { get; set; }

        /// <summary>
        /// api ID
        /// </summary>
        public long? PermissionId { get; set; }

        /// <summary>
        ///  ID
        /// </summary>
        public virtual Permission Permission { get; set; }

    }
}
