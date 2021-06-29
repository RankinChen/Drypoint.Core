using Drypoint.Model.Base;
using Drypoint.Model.Base.Auditing;
using FreeSql.DataAnnotations;
using System;

namespace Drypoint.Model.Models
{
    public class PasswordLib : Entity, ICreationAudited<long>, IModificationAudited<long>, ISoftDelete
    {
        /// <summary>
        /// URL
        /// </summary>
        [Column(StringLength = 200)]
        public string URL { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Column(StringLength = 100)]
        public string Password { get; set; }

        /// <summary>
        /// 账号名
        /// </summary>
        [Column(StringLength = 200)]
        public string AccountName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 错误次数
        /// </summary>
        public int? ErrorCount { get; set; }

        /// <summary>
        /// 密码提示
        /// </summary>
        [Column(StringLength = 200)]
        public string HintPwd { get; set; }

        /// <summary>
        /// 密码提示问题
        /// </summary>
        [Column(StringLength = 200)]
        public string HintQuestion { get; set; }

        /// <summary>
        /// 最后错误时间
        /// </summary>
        public DateTime? LastErrTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? ModifierUserId { get; set; }
        public DateTime? ModificationTime { get; set; }
    }
}
