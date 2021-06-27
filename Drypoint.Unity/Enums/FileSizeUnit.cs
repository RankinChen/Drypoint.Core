using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Drypoint.Unity.Enums
{
    /// <summary>
    /// 文件大小单位
    /// </summary>
    public enum FileSizeUnit
    {
        /// <summary>
        /// 字节
        /// </summary>
        [Description("B")]
        Byte,

        /// <summary>
        /// K字节
        /// </summary>
        [Description("KB")]
        KB,

        /// <summary>
        /// M字节
        /// </summary>
        [Description("MB")]
        MB,

        /// <summary>
        /// G字节
        /// </summary>
        [Description("GB")]
        GB
    }
}
