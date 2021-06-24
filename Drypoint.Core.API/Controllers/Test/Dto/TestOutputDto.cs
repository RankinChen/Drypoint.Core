using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drypoint.Core.Controllers.Test.Dto
{
    /// <summary>
    /// 测试Swagger 出口参数注释 TestOutputDto
    /// </summary>
    public class TestOutputDto
    {
        /// <summary>
        /// 字符串集合
        /// </summary>
        public IList<string> StringList { get; set; }

        /// <summary>
        /// Guid值
        /// </summary>
        public Guid GuidValue { get; set; }
    }
}
