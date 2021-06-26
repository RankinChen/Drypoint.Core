using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Model.Base
{
    public interface IEntity<TKey>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        TKey Id { get; set; }
    }

    /// <summary>
    /// 默认long类型
    /// </summary>
    public interface IEntity : IEntity<long>
    {

    }
}
