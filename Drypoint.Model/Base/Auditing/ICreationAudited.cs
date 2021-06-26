﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Model.Base.Auditing
{
    /// <summary>
    /// 包含创建操作用户的Id 和创建时间
    /// </summary>
    public interface ICreationAudited<TKey> where TKey : struct
    {
        /// <summary>
        /// 创建用户Id
        /// </summary>
        long? CreatorUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreationTime { get; set; }
    }
}
