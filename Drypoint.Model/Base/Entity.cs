using Drypoint.Unity.Attributes;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Drypoint.Model.Base
{
    public class Entity<TKey> : IEntity<TKey>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Description("主键Id")]
        [Snowflake]
        [Column(Position = 1, IsIdentity = false, IsPrimary = true)]
        public virtual TKey Id { get; set; }
    }

    public class Entity : Entity<long>
    {
    }
}
