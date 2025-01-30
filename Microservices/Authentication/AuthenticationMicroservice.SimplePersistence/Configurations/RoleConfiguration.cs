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
        }
    }
}
