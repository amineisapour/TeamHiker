using AuthenticationMicroservice.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationMicroservice.SimplePersistence.Configurations
{
    internal sealed class UserRoleConfigurationConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            #region Initial
            UserRole userRoleData = new UserRole
            {
                Id = Guid.Parse("947C03C8-7409-4D57-8005-38D594209DDF"),
                RoleId = Guid.Parse("911A51DA-3673-421C-BC66-8EE069EDDCA9"),
                UserId = Guid.Parse("4315F7CF-B05E-496D-9CB9-9D256AA6BB38"),
            };
            builder.HasData(data: userRoleData);
            #endregion
        }
    }
}
