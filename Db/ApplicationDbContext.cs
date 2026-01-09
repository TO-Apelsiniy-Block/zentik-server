using Microsoft.EntityFrameworkCore;

namespace ZenticServer.Db;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User.Model> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureUser(modelBuilder);
    }

    private void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User.Model>(entity =>
        {
            entity.HasKey(u => u.UserId);
            entity.ToTable("user");
            entity.Property(u => u.UserId).HasColumnName("user_id");
            entity.Property(u => u.Username).HasColumnName("username");
            entity.Property(u => u.Email).HasColumnName("email");
            entity.Property(u => u.Password).HasColumnName("password");
        });
        
    }
}
