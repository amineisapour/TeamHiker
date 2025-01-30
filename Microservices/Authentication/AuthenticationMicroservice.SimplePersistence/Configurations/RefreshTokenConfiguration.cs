using AuthenticationMicroservice.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationMicroservice.SimplePersistence.Configurations
{
    internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder
                .HasIndex(current => new { current.UserId })
                .IsUnique(unique: false)
                ;
        }
    }
}
