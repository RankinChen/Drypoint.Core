using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Drypoint.Model.Base.Auditing
{
    public class ModificationAudited<Tkey> : Entity<Tkey>, IModificationAudited<Tkey> where Tkey : struct
    {
        /// <summary>
        /// 创建者Id
        /// </summary>
        [Description("创建者Id")]
        [Column(Position = -2, CanUpdate = false)]
        public long? ModifierUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        [Column(Position = -1, CanUpdate = false, ServerTime = DateTimeKind.Local)]
        public DateTime? ModificationTime { get; set; }
    }

    public class ModificationAudited : ModificationAudited<long>
    {

    }
}
