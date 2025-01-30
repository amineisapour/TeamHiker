using AuthenticationMicroservice.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationMicroservice.SimplePersistence.Configurations
{
    internal sealed class UserInformationConfiguration : IEntityTypeConfiguration<UserInformation>
    {
        public void Configure(EntityTypeBuilder<UserInformation> builder)
        {
            builder
                .HasIndex(current => new { current.UserId })
                .IsUnique(unique: true)
                ;

            builder.Property(m => m.UserId).IsRequired();
            builder.HasOne(m => m.User).WithOne().IsRequired();
        }
    }
}
