using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Unity.OptionsConfigModels
{
    public class InitDBTaskConfig
    {
        /// <summary>
        /// 生成数据
        /// </summary>
        public bool GenerateData { get; set; } = false;

        /// <summary>
        /// 同步结构
        /// </summary>
        public bool SyncStructure { get; set; } = true;

        /// <summary>
        /// 同步数据
        /// </summary>
        public bool SyncData { get; set; } = true;

        /// <summary>
        /// 建库
        /// </summary>
        public bool CreateDB { get; set; } = true;

        /// <summary>
        /// 建库连接字符串
        /// </summary>
        public string CreateDBConnectionString { get; set; }

        /// <summary>
        /// 建库脚本
        /// </summary>
        public string CreateDBSql { get; set; }
    }
}
