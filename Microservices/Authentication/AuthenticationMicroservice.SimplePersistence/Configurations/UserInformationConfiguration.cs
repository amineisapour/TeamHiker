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

            #region Initial
            UserInformation userInformationData = new UserInformation
            {
                Id = Guid.Parse("3DAE8BAB-437B-44FA-B595-BF7E90B5CCA9"),
                UserId = Guid.Parse("4315F7CF-B05E-496D-9CB9-9D256AA6BB38"),
                FirstName = "Main",
                LastName = "Admin",
                Gender = Domain.Enums.Gender.Man,
                NationalId = "1234567890",
                Birthdate = DateTime.Now,
                Phone = "+989123456789",
                Email = "admin@teamhiker.com",
                Bio = "Bio",
                Language = "English",
            };
            builder.HasData(data: userInformationData);
            #endregion
        }
    }
}
