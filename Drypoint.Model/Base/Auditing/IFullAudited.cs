using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Model.Base.Auditing
{
    public interface IFullAudited<TKey> : IAudited<TKey>, IDeletionAudited<TKey> where TKey : struct
    {

    }
}
