using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.EntityFrameworkCore.Repositories
{
    public interface IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 主键（唯一标识）
        /// </summary>
        TPrimaryKey Id { get; set; }

        /// <summary>
        /// 检查这个实体是否是临时的(待用)
        /// </summary>
        /// <returns>如果这个实体是暂时的，则为True</returns>
        bool IsTransient();
    }
}
