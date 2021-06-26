using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Drypoint.Model.Base.Auditing
{

    [Serializable]
    public abstract class CreationAuditedEntity<TKey> : Entity<TKey>, ICreationAudited<TKey> where TKey : struct
    {
        /// <summary>
     /// 创建者Id
     /// </summary>
        [Description("创建者Id")]
        [Column(Position = -2, CanUpdate = false)]
        public long? CreatorUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        [Column(Position = -1, CanUpdate = false, ServerTime = DateTimeKind.Local)]
        public DateTime CreationTime { get; set; }
    }

    public class CreationAuditedEntity : CreationAuditedEntity<long>
    {
    }
}
