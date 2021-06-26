using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Model.Base.Auditing
{
    public interface IDeletionAudited<TKey> where TKey : struct
    {
        /// <summary>
        /// 删除用户Id
        /// </summary>
        long? DeleterUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        DateTime? DeletionTime { get; set; }
    }
}
