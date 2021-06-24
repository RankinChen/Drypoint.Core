using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drypoint.Core.Controllers.Test.Dto
{
    /// <summary>
    /// 测试Swagger 入口参数注释 TestInputDto
    /// </summary>
    public class TestInputDto
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// bool类型
        /// </summary>
        public bool IsBoolValue { get; set; }
    }
}
