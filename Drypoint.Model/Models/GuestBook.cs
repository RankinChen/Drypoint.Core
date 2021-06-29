using Drypoint.Model.Base;
using Drypoint.Model.Base.Auditing;
using FreeSql.DataAnnotations;
using System;

namespace Drypoint.Model.Models
{
    public class GuestBook : Entity,ICreationAudited<long>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Column(StringLength = 200)]
        public string UserName { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [Column(StringLength = 18)]
        public string Phone { get; set; }

        /// <summary>
        /// qq
        /// </summary>
        [Column(StringLength = 20)]
        public string QQ { get; set; }

        /// <summary>
        /// 留言内容
        /// </summary>
        [Column(StringLength = 2000)]
        public string Content { get; set; }

        /// <summary>
        /// ip地址
        /// </summary>
        [Column(StringLength = 20)]
        public string IPAddress { get; set; }

        /// <summary>
        /// 是否显示在前台,0否1是
        /// </summary>
        public bool IsShow { get; set; }

        /// <summary>
        /// 博客ID
        /// </summary>
        public long? BlogArticleId { get; set; }

        public virtual BlogArticle BlogArticle { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public long? CreatorUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
