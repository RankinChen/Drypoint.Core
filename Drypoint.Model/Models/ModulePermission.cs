using Drypoint.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Model.Models
{
    /// <summary>
    /// 菜单与按钮关系表
    /// </summary>
    public class ModulePermission : EntityFull<long>
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public long ModuleId { get; set; }
        /// <summary>
        /// 按钮ID
        /// </summary>
        public long PermissionId { get; set; }
    }
}
