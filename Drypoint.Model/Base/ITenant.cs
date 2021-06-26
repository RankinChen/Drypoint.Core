using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Model.Base
{
    public interface ITenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        long? TenantId { get; set; }
    }
}
