using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Application.Services.Dto.Interface
{
    /// <summary>
    /// 最大取条数
    /// </summary>
    public interface ILimitedBaseRequest
    {
        /// <summary>
        /// 最大取得行数
        /// </summary>
        int MaxResultCount { get; set; }
    }
}
