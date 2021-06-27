using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Unity.SharedModels
{
    public class PageQueryModel<T>
    {
        /// <summary>
        /// 当前页标
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { set; get; } = 50;

        /// <summary>
        /// 查询条件
        /// </summary>
        public T Filter { get; set; }
    }
}
