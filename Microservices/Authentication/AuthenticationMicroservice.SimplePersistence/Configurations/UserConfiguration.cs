using AuthenticationMicroservice.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationMicroservice.SimplePersistence.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasIndex(current => new { current.Username })
                .IsUnique(unique: true)
                ;

            builder.HasOne(m => m.UserInformations).WithOne().IsRequired();
        }
    }
}
