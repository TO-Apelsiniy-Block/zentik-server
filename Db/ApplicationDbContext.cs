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
    public DbSet<Chat.Model> Chats { get; set; }
    public DbSet<Message.Model> Messages { get; set; }
    public DbSet<EmailConfirmation.Model> EmailConfirmations { get; set; }
    public DbSet<Chat.ChatUser.Model> ChatUsers { get; set; }
    public DbSet<Message.Type.TextModel> MessageTexts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureUser(modelBuilder);
        ConfigureEmailConfirmation(modelBuilder);
        ConfigureChat(modelBuilder);
        ConfigureMessage(modelBuilder);

        ConfigureBaseFields(modelBuilder);
    }

    private void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User.Model>(entity =>
        {
            entity.HasKey(z => z.UserId);
            entity.ToTable("user");
            entity.Property(z => z.UserId).HasColumnName("user_id");
            entity.Property(z => z.Username).HasColumnName("username").IsRequired();
            entity.Property(z => z.Email).HasColumnName("email").IsRequired();
            entity.HasIndex(z => z.Email).IsUnique();
            entity.Property(z => z.Password).HasColumnName("password").IsRequired();
        });
    }
    
    private void ConfigureEmailConfirmation(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmailConfirmation.Model>(entity =>
        {
            entity.HasKey(z => new { z.Email, z.DeviceId });
            entity.ToTable("email_confirmation");
            entity.Property(z => z.Code).HasColumnName("code").IsRequired();
            entity.Property(z => z.Email).HasColumnName("email").IsRequired();
            entity.Property(z => z.DeviceId).HasColumnName("device_id").IsRequired();
        });
    }
    
    private void ConfigureChat(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat.Model>(entity =>
        {
            entity.HasKey(z => z.ChatId);
            entity.ToTable("chat");
            entity.Property(z => z.ChatId).HasColumnName("chat_id");
            entity.Property(z => z.Type).HasColumnName("type").IsRequired();
            
            entity.HasMany(z => z.Messages)
                .WithOne(z => z.Chat)
                .HasForeignKey(z => z.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(z => z.Users)
                .WithMany(z => z.Chats)
                .UsingEntity<Chat.ChatUser.Model>(entity =>
                {
                    entity.HasKey(z => new { z.UserId, z.ChatId });
                    entity.ToTable("chat_user");
                    entity.Property(z => z.ChatId).HasColumnName("chat_id").IsRequired();
                    entity.Property(z => z.UserId).HasColumnName("user_id").IsRequired();
                    entity.Property(z => z.Role).HasColumnName("role").IsRequired();
                    entity.HasOne(z => z.Chat)
                        .WithMany(z => z.ChatUsers)
                        .HasForeignKey(z => z.ChatId)
                        .OnDelete(DeleteBehavior.Cascade);
                    entity.HasOne(z => z.User)
                        .WithMany(z => z.ChatUsers)
                        .HasForeignKey(z => z.UserId)
                        .OnDelete(DeleteBehavior.Cascade);
                });
        });
    }
    
    private void ConfigureMessage(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message.Model>(entity =>
        {
            entity.HasKey(z => z.MessageId);
            entity.ToTable("message");
            entity.Property(z => z.Type).HasColumnName("type").IsRequired();
            entity.Property(z => z.ChatId).HasColumnName("chat_id").IsRequired();
            entity.Property(z => z.SenderId).HasColumnName("sender_id").IsRequired();
            entity.Property(z => z.MessageId).HasColumnName("message_id");

            entity.HasOne(z => z.Chat)
                .WithMany(z => z.Messages)
                .HasForeignKey(z => z.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(z => z.Sender)
                .WithMany(z => z.Messages)
                .HasForeignKey(z => z.SenderId)
                .OnDelete(DeleteBehavior.Cascade); // TODO изменить удаление аккаунта на деактивацию
            entity.HasOne(d => d.Text)
                .WithOne(d => d.Message)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey<Message.Type.TextModel>(e => e.MessageId);
        });

        modelBuilder.Entity<Message.Type.TextModel>(entity =>
        {
            entity.HasOne(d => d.Message)
                .WithOne(d => d.Text)
                .HasForeignKey<Message.Type.TextModel>(e => e.MessageId)
                .IsRequired();
            entity.HasKey(z => z.MessageId);
            entity.ToTable("message_text");
            entity.Property(z => z.Text).HasColumnName("text").IsRequired();
            
        });
    }

    private void ConfigureBaseFields(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                     .Where(e => typeof(Base.Model).IsAssignableFrom(e.ClrType)))
        {
            modelBuilder.Entity(entityType.ClrType)
                .Property<DateTime>("CreatedAt").HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity(entityType.ClrType)
                .Property<DateTime>("UpdatedAt").HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
