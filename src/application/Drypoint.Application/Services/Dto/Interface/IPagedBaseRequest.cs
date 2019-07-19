using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Application.Services.Dto.Interface
{
    /// <summary>
    /// 跳过多少条
    /// </summary>
    public interface IPagedBaseRequest:ILimitedBaseRequest
    {
        /// <summary>
        /// 跳过的行数
        /// </summary>
        int SkipCount { get; set; }
    }
}
