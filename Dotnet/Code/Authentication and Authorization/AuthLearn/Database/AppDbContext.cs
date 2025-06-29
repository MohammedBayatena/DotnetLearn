using AuthLearn.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthLearn.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "AuthLearn");
    }

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshTokenInfoEntity> TokensInfo { get; set; }
    public DbSet<SessionBlackListEntity> CookiesBlackList { get; set; }
    public DbSet<TokenBlackListEntity> TokensBlackList { get; set; }
    public DbSet<UserPermissionEntity> Permissions { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserPermissionEntity>()
            .HasOne(e => e.User)
            .WithOne(e => e.Permissions)
            .HasForeignKey<UserPermissionEntity>(e => e.UserId)
            .IsRequired();
    }
}