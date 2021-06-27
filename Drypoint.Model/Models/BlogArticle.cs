using Drypoint.Model.Base;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Model.Models
{
    /// <summary>
    /// 博客文章
    /// </summary>
    public class BlogArticle : EntityFull<long>
    {
        /// <summary>
        /// 标题blog
        /// </summary>
        [Column(StringLength = 256)]
        public string Title { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        [Column(StringLength = 100)]
        public string Category { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Column(StringLength = 2000)]
        public string Content { get; set; }

        /// <summary>
        /// 访问量
        /// </summary>
        public int Traffic { get; set; }

        /// <summary>
        /// 评论数量
        /// </summary>
        public int CommentNum { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column(StringLength = 2000, IsNullable = true)]
        public string Remark { get; set; }
    }
}
