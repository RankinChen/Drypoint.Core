using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Model.Models.Admin
{
    /// <summary>
    /// 系统表，用于查询系统函数
    /// </summary>
    [Table(Name = "Sys_Dual")]
    public class DualEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Column(Position = 1, IsPrimary = true, IsNullable = false)]
        public Guid Id { get; set; }
    }
}
