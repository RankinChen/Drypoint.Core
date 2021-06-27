using Drypoint.Model.Base;
using Drypoint.Model.Base.Auditing;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Model.Models
{
    public class Advertisement: Entity<long>, ICreationAudited<long>
    {
        /// <summary>
        /// 广告图片
        /// </summary>
        [Column(StringLength = 512, IsNullable = true)]
        public string ImgUrl { get; set; }

        /// <summary>
        /// 广告标题
        /// </summary>
        [Column(StringLength = 64, IsNullable = true)]
        public string Title { get; set; }

        /// <summary>
        /// 广告链接
        /// </summary>
        [Column(StringLength = 256, IsNullable = true)]
        public string Url { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column(StringLength = 2000, IsNullable = true)]
        public string Remark { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public long? CreatorUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
