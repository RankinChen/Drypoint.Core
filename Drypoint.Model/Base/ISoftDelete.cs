namespace Drypoint.Model.Base
{
    /// <summary>
    /// 包含 逻辑删除属性
    /// </summary>
    public interface ISoftDelete : IEntityFlag
    {
        /// <summary>
        /// 逻辑删除
        /// </summary>
        bool IsDeleted { get; set; }
    }
}
