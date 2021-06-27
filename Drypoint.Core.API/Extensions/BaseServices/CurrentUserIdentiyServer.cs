using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Core.Extensions.BaseServices
{
    /// <summary>
    /// 当前用户信息
    /// </summary>
    public class CurrentUserIdentiyServer : CurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public CurrentUserIdentiyServer(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public override long Id
        {
            get
            {
                //TODO
                var id = _httpContextAccessor?.HttpContext?.User?.FindFirst("id");
                if (id!=null && string.IsNullOrEmpty(id.Value))
                {
                    return Convert.ToInt64(id.Value);
                }
                return 0;
            }
        }
    }
}
