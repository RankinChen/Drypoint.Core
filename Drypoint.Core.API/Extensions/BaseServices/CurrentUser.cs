using Drypoint.Unity.BaseServices;
using Drypoint.Unity.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Core.Extensions.BaseServices
{
    /// <summary>
    /// 当前用户信息
    /// </summary>
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual long Id
        {
            get
            {
                //TODO
                var id = _httpContextAccessor?.HttpContext?.User?.FindFirst("id");
                if (id != null && string.IsNullOrEmpty(id.Value))
                {
                    return Convert.ToInt64(id.Value);
                }
                return 0;
            }
        }

        public string Name => throw new NotImplementedException();

        public string NickName => throw new NotImplementedException();

        public long? TenantId => throw new NotImplementedException();

        public TenantType? TenantType => throw new NotImplementedException();
    }
}
