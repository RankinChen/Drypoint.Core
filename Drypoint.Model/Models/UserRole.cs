using Drypoint.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Model.Models
{
    public class UserRole:EntityFull
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public virtual Role Role { get; set; }
    }
}
