using AuthenticationMicroservice.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationMicroservice.SimplePersistence.Configurations
{
    internal sealed class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.RegisterDateTime).IsRequired();
            builder.Property(m => m.Fullname).IsRequired().IsUnicode().HasMaxLength(254);
            builder.Property(m => m.Message).IsRequired();
            builder.Property(m => m.Email).IsRequired().HasMaxLength(254);
        }
    }
}
