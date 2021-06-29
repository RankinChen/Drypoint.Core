namespace Drypoint.Model.Base.Auditing
{
    public interface IAudited<TKey> : ICreationAudited<TKey>, IModificationAudited<TKey> where TKey : struct
    {
    }
}
