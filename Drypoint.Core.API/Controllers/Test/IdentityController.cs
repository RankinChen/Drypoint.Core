using Drypoint.Core.Controllers.Base;
using Drypoint.Core.Controllers.Test.Dto;
using Drypoint.Unity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drypoint.Core.Controllers.Test
{
    /// <summary>
    /// 测试Swagger接口 IdentityController
    /// </summary>
    public class IdentityController : AppBaseController
    {
        /// <summary>
        /// Identity的Get接口
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public TestOutputDto Get(TestInputDto dto)
        {
            var data = new JsonResult(
                from c in User.Claims select new { c.Type, c.Value });

            return new TestOutputDto();
        }
    }
}
