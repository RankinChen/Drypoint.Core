using Drypoint.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Model.Models
{
    /// <summary>
    /// 路由菜单表
    /// </summary>
    public class Permission:Entity<long>
    {
        /// <summary>
        /// 上一级Id（null表示上一级无菜单）
        /// </summary>
        public long? ParentId { get; set; }
        /// <summary>
        /// 上一级菜单（null表示上一级无菜单）
        /// </summary>
        public virtual Permission Parent { get; set; }

        /// <summary>
        /// 接口api
        /// </summary>
        public long ModuleId { get; set; }

        /// <summary>
        /// 接口api
        /// </summary>
        public virtual Module Module{ get; set; }
    }
}
