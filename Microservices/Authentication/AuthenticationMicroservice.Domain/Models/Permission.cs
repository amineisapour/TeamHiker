using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationMicroservice.Domain.Models
{
    public class Permission : Base.Entity
    {
        public Permission() : base()
        {
            UserPermissions = new List<UserPermission>();
            RolePermissions = new List<RolePermission>();
        }

        [Required]
        [MaxLength(254)]
        public string Name { get; set; }

        public virtual IList<UserPermission> UserPermissions { get; private set; }
        
        public virtual IList<RolePermission> RolePermissions { get; private set; }
    }
}
