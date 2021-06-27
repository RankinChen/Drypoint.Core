using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Model.Base.Auditing
{
    public interface IAudited<TKey> : ICreationAudited<TKey>, IModificationAudited<TKey> where TKey : struct
    {
    }
}
