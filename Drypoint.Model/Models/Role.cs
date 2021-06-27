using Drypoint.Model.Base;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Model.Models
{
    /// <summary>
    /// 角色表
    /// </summary>
    public class Role : EntityFull<long>, IPassivable
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [Column(StringLength = 50)]
        public string Name { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        [Column(StringLength = 100)]
        public string Description { get; set; }

        /// <summary>
        ///排序
        /// </summary>
        public int OrderSort { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }
    }
}
