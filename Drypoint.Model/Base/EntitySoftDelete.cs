using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Drypoint.Model.Base
{
    /// <summary>
    /// 实体软删除
    /// </summary>
    public class EntitySoftDelete<TKey> : Entity<TKey>, ISoftDelete
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        [Description("是否删除")]
        [Column(Position = -1)]
        public bool IsDeleted { get; set; } = false;
    }

    public class EntitySoftDelete : EntitySoftDelete<long>
    {
    }
}
