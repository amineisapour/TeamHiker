using AuthenticationMicroservice.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationMicroservice.SimplePersistence.Configurations
{
    internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            #region Initial
            RolePermission rolePermissionData = new RolePermission
            {
                Id = Guid.Parse("1A3C593E-FCDD-4EA1-81C6-B572C6DF97CF"),
                RoleId = Guid.Parse("911A51DA-3673-421C-BC66-8EE069EDDCA9"),
                PermissionId = Guid.Parse("8C89C181-886A-4511-9027-8CF84BD914F3"),
            };
            builder.HasData(data: rolePermissionData);
            #endregion
        }
    }
}
