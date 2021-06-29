using Drypoint.Model.Base;
using Drypoint.Unity.Enums;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Model.Models
{
    public class User : EntityFull, IPassivable
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        [Column( StringLength = 200)]
        public string UserName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [Column(StringLength = 200)]
        public string Password { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Column( StringLength = 200)]
        public string RealName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column(StringLength = 2000)]
        public string Remark { get; set; }

        /// <summary>
        /// 最后登录时间 
        /// </summary>
        public DateTime LastLoginTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 错误次数 
        /// </summary>
        public int ErrorCount { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public GenderEnum? Gender { get; set; } = 0;

        /// <summary>
        /// 生日
        /// </summary>       
        public DateTime? Birthday { get; set; } = DateTime.Now;

        /// <summary>
        /// 地址
        /// </summary>
        [Column(StringLength = 200)]
        public string Address { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }
    }
}
