using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AuthenticationMicroservice.Domain.Models
{
    public class Role : Base.Entity
    {
        public Role() : base()
        {
            RolePermissions = new List<RolePermission>();
            UserRoles = new List<UserRole>();
        }

        //private readonly ILazyLoader _loader;
        //public Role(ILazyLoader loader)
        //{
        //    _loader = loader;
        //}

        //private List<RolePermission> _rolePermissions;
        //[Required]
        //public virtual List<RolePermission> RolePermissions
        //{
        //    get => _loader.Load(this, ref _rolePermissions);
        //    set => _rolePermissions = value;
        //}

        [Required]
        [MaxLength(254)]
        public string Name { get; set; }

        public virtual IList<RolePermission> RolePermissions { get; private set; }

        public virtual IList<UserRole> UserRoles { get; private set; }
        
    }
}
