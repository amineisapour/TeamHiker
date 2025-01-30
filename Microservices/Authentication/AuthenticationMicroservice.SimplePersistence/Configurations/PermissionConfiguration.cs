using AuthenticationMicroservice.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
        }
    }
}
