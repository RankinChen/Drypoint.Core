namespace Drypoint.Model.Base
{
    /// <summary>
    /// 含是否激活属性
    /// </summary>
    public interface IPassivable : IEntityFlag
    {
        /// <summary>
        /// 是否激活
        /// </summary>
        bool IsActive { get; set; }
    }
}
