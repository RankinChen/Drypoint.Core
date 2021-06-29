using Drypoint.Model.Base;
using FreeSql.DataAnnotations;

namespace Drypoint.Model.Models
{
    /// <summary>
    /// 接口API地址信息表
    /// </summary>
    public class Module: EntityFull
    {
        /// <summary>
        /// 父ID
        /// </summary>
        [Column(IsNullable = true)]
        public long? ParentId { get; set; }
        public virtual Module Parent { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column(StringLength = 50)]
        public string Name { get; set; }

        /// <summary>
        /// 菜单链接地址
        /// </summary>
        [Column(StringLength = 100)]
        public string LinkUrl { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        [Column(StringLength = 2000)]
        public string Area { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        [Column(StringLength = 2000)]
        public string Controller { get; set; }

        /// <summary>
        /// Action名称
        /// </summary>
        [Column(StringLength = 2000)]
        public string Action { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [Column(StringLength = 100)]
        public string Icon { get; set; }

        /// <summary>
        /// 菜单编号
        /// </summary>
        [Column(StringLength = 10)]
        public string Code { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderSort { get; set; }

        /// <summary>
        /// /描述
        /// </summary>
        [Column(StringLength = 100)]
        public string Description { get; set; }

        /// <summary>
        /// 是否是右侧菜单
        /// </summary>
        public bool IsMenu { get; set; }
    }
}
