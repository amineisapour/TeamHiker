using AuthenticationMicroservice.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationMicroservice.SimplePersistence.Configurations
{
    internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(m => m.RegisterDateTime).IsRequired();
            builder.Property(m => m.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(m => m.Name).IsRequired().IsUnicode().HasMaxLength(254);

            #region Initial
            Role roleData = new Role
            {
                Id = Guid.Parse("911A51DA-3673-421C-BC66-8EE069EDDCA9"),
                Name = "Admin"
            };
            builder.HasData(data: roleData);

            roleData = new Role
            {
                Id = Guid.Parse("8F776629-1DA0-4E59-A951-34FF9A898DE4"),
                Name = "User"
            };
            builder.HasData(data: roleData);
            #endregion
        }
    }
}
