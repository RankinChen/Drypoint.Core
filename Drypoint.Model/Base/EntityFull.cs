using Drypoint.Model.Base.Auditing;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Drypoint.Model.Base
{
    public class EntityFull<TKey> : Entity<TKey>, ISoftDelete, IFullAudited<TKey> where TKey : struct
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        [Description("是否删除")]
        [Column(Position = -7)]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// 创建者Id
        /// </summary>
        [Description("创建者Id")]
        [Column(Position = -6, CanUpdate = false)]
        public long? CreatorUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        [Column(Position = -5, CanUpdate = false, ServerTime = DateTimeKind.Local)]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 修改者Id
        /// </summary>
        [Description("修改者Id")]
        [Column(Position = -4, CanInsert = false)]
        public long? ModifierUserId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Description("修改时间")]
        [Column(Position = -3, CanInsert = false, ServerTime = DateTimeKind.Local)]
        public DateTime? ModificationTime { get; set; }

        /// <summary>
        /// 删除者Id
        /// </summary>
        [Description("删除者Id")]
        [Column(Position = -2, CanInsert = false)]
        public long? DeleterUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        [Description("删除时间")]
        [Column(Position = -1, CanInsert = false, ServerTime = DateTimeKind.Local)]
        public DateTime? DeletionTime { get; set; }
    }


    public class EntityFull : EntityFull<long>
    {
    }
}
