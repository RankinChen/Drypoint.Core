using Drypoint.Unity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Unity.Auth
{
    /// <summary>
    /// 用户信息接口
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// 主键
        /// </summary>
        long Id { get; }

        /// <summary>
        /// 用户名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 昵称
        /// </summary>
        string NickName { get; }

        /// <summary>
        /// 租户Id
        /// </summary>
        long? TenantId { get; }

        /// <summary>
        /// 租户类型
        /// </summary>
        TenantType? TenantType { get; }
    }
}
