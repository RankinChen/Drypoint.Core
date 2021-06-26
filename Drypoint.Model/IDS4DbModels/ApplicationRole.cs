using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Model.IDS4DbModels
{
    [Table(Name = "AspNetRoles", DisableSyncStructure = true)]
    public class ApplicationRole
    {
        public string Description { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? ModifierUserId { get; set; }
        public DateTime? ModificationTime { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
