using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Unity
{
    public static class DrypointConsts
    {
        #region 数据库连接字符串 Key
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public const string Default_ConnectionStringName = "Default";

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public const string IdsDB_ConnectionStringName = "IdsDB";
        #endregion

        #region 接口定义相关
        /// <summary>
        /// 接口前缀
        /// </summary>
        public const string ApiPrefix = "api/";

        /// <summary>
        /// admin 接口组名
        /// </summary>
        public const string AdminAPIGroupName = "Admin";

        /// <summary>
        /// app 接口组名
        /// </summary>
        /// 
        public const string AppAPIGroupName = "App";


        public static readonly IReadOnlyCollection<string> ApiGroups = new List<string> { AppAPIGroupName, AdminAPIGroupName };

        #endregion

        #region 自定义授权相关scope
        public const string RolesScope = "roles";
        #endregion

        /// <summary>
        /// 列表数据 分页 每页最多显示条数
        /// </summary>
        public const int MaxPageSize = 1000;

        /// <summary>
        /// 列表数据 每页条数
        /// </summary>
        public const int DefaultPageSize = 10;


        /// <summary>
        ///     Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public const string DefaultPassPhrase = "(*^▽^*)";

        public const string CacheKey_TokenValidityKey = "token_validity_key";
    }
}
