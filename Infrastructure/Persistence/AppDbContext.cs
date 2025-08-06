using Microsoft.EntityFrameworkCore;
using Domain.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            // Primary Key
            entity.HasKey(u => u.Id);

            // Properties
            entity.Property(u => u.NickName)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(u => u.Password)
                .IsRequired();

            entity.Property(u => u.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(u => u.UpdatedAt)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(u => u.DeletedAt)
                .HasColumnType("timestamp with time zone")
                .IsRequired(false);

            // Indexes
            entity.HasIndex(u => u.Email)
                .IsUnique();

            // Relationships
            entity.HasMany(u => u.OwnedChatRooms)
                .WithOne(cr => cr.Owner)
                .HasForeignKey(cr => cr.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(u => u.ChatRoomMemberships)
                .WithOne(crm => crm.User)
                .HasForeignKey(crm => crm.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(u => u.SentMessages)
                .WithOne(m => m.Sender)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ChatRoom>(entity =>
        {
            // Primary Key
            entity.HasKey(cr => cr.Id);

            // Properties
            entity.Property(cr => cr.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(cr => cr.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(cr => cr.UpdatedAt)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(cr => cr.DeletedAt)
                .HasColumnType("timestamp with time zone")
                .IsRequired(false);

            // Relationships
            entity.HasOne(cr => cr.Owner)
                .WithMany(u => u.OwnedChatRooms)
                .HasForeignKey(cr => cr.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(cr => cr.Members)
                .WithOne(crm => crm.ChatRoom)
                .HasForeignKey(crm => crm.ChatRoomId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(cr => cr.Messages)
                .WithOne(m => m.ChatRoom)
                .HasForeignKey(m => m.ChatRoomId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ChatRoomMember>(entity =>
        {
            // Composite Key
            entity.HasKey(crm => new { crm.UserId, crm.ChatRoomId });

            // Properties
            entity.Property(crm => crm.JoinedAt)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(crm => crm.LeftAt)
                .HasColumnType("timestamp with time zone")
                .IsRequired(false);

            // Relationships
            entity.HasOne(crm => crm.User)
                .WithMany(u => u.ChatRoomMemberships)
                .HasForeignKey(crm => crm.UserId);

            entity.HasOne(crm => crm.ChatRoom)
                .WithMany(cr => cr.Members)
                .HasForeignKey(crm => crm.ChatRoomId);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            // Primary Key
            entity.HasKey(m => m.Id);

            // Properties
            entity.Property(m => m.Content)
                .IsRequired()
                .HasMaxLength(6000);

            entity.Property(m => m.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(m => m.UpdatedAt)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(m => m.DeletedAt)
                .HasColumnType("timestamp with time zone")
                .IsRequired(false);

            // Indexes
            entity.HasIndex(m => m.ChatRoomId);
            entity.HasIndex(m => m.SenderId);
            entity.HasIndex(m => m.CreatedAt);

            // Relationships
            entity.HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.ChatRoom)
                .WithMany(cr => cr.Messages)
                .HasForeignKey(m => m.ChatRoomId)
                .OnDelete(DeleteBehavior.Restrict);


        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<User>();


        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }

        }
        
        return await base.SaveChangesAsync(cancellationToken);
    }
}