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

            #region Initial
            string passwordHash = "D2099AC355F75F82F1339FB36B4D90F92A913573AAAADD0672553DF629E2EC97886126B2DDCA77C4237C44C4D05290B4B5FBB67EE509B94B232756BCE5F83C20";
            string passwordSalt = "2E040917A179AE9D43FDFC68E3D39E43E2CD8995367C0DF3327B11CBCFC7EAA9FFCCA865B0957178173132BEC6BC20386E4B70440215426BB2B868245509F1EC2CE78312A2E074E6EDBF078EDD75655733CDE62BE4453448DCC4BFFA173095C13E53DFF7F072D9BE874DF6C1BA670C5CDC41EB7A9A679E849F1D2A6CE7326494";
            User userData = new User
            {
                Id = Guid.Parse("4315F7CF-B05E-496D-9CB9-9D256AA6BB38"),
                Username = "admin@teamhiker.com",
                PasswordHash = ConvertHexStringToByteArray(passwordHash),
                PasswordSalt = ConvertHexStringToByteArray(passwordSalt),
            };
            builder.HasData(data: userData);
            #endregion
        }

        private static byte[] ConvertHexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length / 2)
                             .Select(i => Convert.ToByte(hex.Substring(i * 2, 2), 16))
                             .ToArray();
        }
    }
}
