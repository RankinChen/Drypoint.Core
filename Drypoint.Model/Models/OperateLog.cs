using Drypoint.Model.Base;
using Drypoint.Model.Base.Auditing;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Model.Models
{
    /// <summary>
    /// 日志操作记录
    /// </summary>
    public class OperateLog : Entity<long>, ICreationAudited<long>
    {
        /// <summary>
        /// 区域名
        /// </summary>
        [Column(StringLength = 100)]
        public string Area { get; set; }
        /// <summary>
        /// 区域控制器名
        /// </summary>
        [Column(StringLength = 200)]
        public string Controller { get; set; }
        /// <summary>
        /// Action名称
        /// </summary>
        [Column(StringLength = 200)]
        public string Action { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        [Column(StringLength = 20)]
        public string IPAddress { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Column(StringLength = 2000)]
        public string Description { get; set; }
        
        /// <summary>
        /// 创建用户Id
        /// </summary>
        public long? CreatorUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
