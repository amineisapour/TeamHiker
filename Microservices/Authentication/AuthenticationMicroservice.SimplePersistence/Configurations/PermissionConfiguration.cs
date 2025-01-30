using AuthenticationMicroservice.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthenticationMicroservice.SimplePersistence.Configurations
{
    internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.RegisterDateTime).IsRequired();
            builder.Property(m => m.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(m => m.Name).IsRequired().IsUnicode().HasMaxLength(254);

            #region Initial
            Permission permissionData = new Permission
            {
                Id = Guid.Parse("EA0E44F1-B7B0-42D8-9851-16C414CABD28"),
                Name = "Permission.Account.CanRead"
            };
            builder.HasData(data: permissionData);

            permissionData = new Permission
            {
                Id = Guid.Parse("D9FD2197-5F88-413E-B3D7-302CB0AE6BC6"),
                Name = "Permission.Account.CanEdit"
            };
            builder.HasData(data: permissionData);

            permissionData = new Permission
            {
                Id = Guid.Parse("8C89C181-886A-4511-9027-8CF84BD914F3"),
                Name = "Permission.Action.FullAccess"
            };
            builder.HasData(data: permissionData);

            permissionData = new Permission
            {
                Id = Guid.Parse("739BC7EF-5EFF-4827-ACD5-D118E03F3658"),
                Name = "Permission.Account.CanDelete"
            };
            builder.HasData(data: permissionData);

            permissionData = new Permission
            {
                Id = Guid.Parse("D49E9A4D-54A9-41E2-823F-EE90BAC788EF"),
                Name = "Permission.Account.CanAdd"
            };
            builder.HasData(data: permissionData);
            #endregion
        }
    }
}
