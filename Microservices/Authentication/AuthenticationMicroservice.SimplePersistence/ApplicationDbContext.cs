using AuthenticationMicroservice.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AuthenticationMicroservice.SimplePersistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() : base()
    {
        //Database.Migrate();
        Database.EnsureCreated();
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options: options)
    {
        //Database.Migrate();
        Database.EnsureCreated();
    }

    #region Table
    public DbSet<Permission> Permissions { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<RolePermission> RolePermissions { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<UserInformation> UserInformations { get; set; }

    public DbSet<UserPermission> UserPermissions { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }

    public DbSet<User> Users { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(assembly: typeof(ApplicationDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured == false)
        {
            var connectionString =
                "Server=.;User ID=sa;Password=1234;Database=TeamHikerDB;MultipleActiveResultSets=true;TrustServerCertificate=True;";

            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(connectionString: connectionString)
                ;
        }
    }
}
