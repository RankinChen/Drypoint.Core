using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Drypoint.Unity.Cache
{
    /// <summary>
    /// 缓存键
    /// </summary>
    public static class CacheKey
    {
        /// <summary>
        /// 验证码 drypoint:verify:code:guid
        /// </summary>
        [Description("验证码")]
        public const string VerifyCodeKey = "drypoint:verify:code:{0}";

        /// <summary>
        /// 密码加密 drypoint:password:encrypt:guid
        /// </summary>
        [Description("密码加密")]
        public const string PassWordEncryptKey = "drypoint:password:encrypt:{0}";

        /// <summary>
        /// 用户权限 drypoint:user:permissions:用户主键
        /// </summary>
        [Description("用户权限")]
        public const string UserPermissions = "drypoint:user:permissions:{0}";

        /// <summary>
        /// 用户信息 drypoint:user:info:用户主键
        /// </summary>
        [Description("用户信息")]
        public const string UserInfo = "drypoint:user:info:{0}";

        /// <summary>
        /// 租户信息 drypoint:tenant:info:租户主键
        /// </summary>
        [Description("租户信息")]
        public const string TenantInfo = "drypoint:tenant:info:{0}";
    }
}
