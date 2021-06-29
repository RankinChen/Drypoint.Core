namespace Drypoint.Model.Base
{
    public interface IEntity<TKey> : IEntityFlag
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        TKey Id { get; set; }
    }

    /// <summary>
    /// 默认long类型
    /// </summary>
    public interface IEntity : IEntity<long>, IEntityFlag
    {

    }
}
