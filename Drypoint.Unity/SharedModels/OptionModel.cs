using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Unity.SharedModels
{
    /// <summary>
    /// 下拉选项输出
    /// </summary>
    public class OptionModel
    {
        /// <summary>
     /// 名称
     /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }
    }
}
