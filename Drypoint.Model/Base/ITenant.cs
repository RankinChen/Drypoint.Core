namespace Drypoint.Model.Base
{
    public interface ITenant : IEntityFlag
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        long? TenantId { get; set; }
    }
}
