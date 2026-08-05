using Microsoft.EntityFrameworkCore;

namespace Play.Infrastructure.Persistence;

public sealed class PlayDbContext(DbContextOptions<PlayDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<UserList> Lists => Set<UserList>();
    public DbSet<ListItem> ListItems => Set<ListItem>();
    public DbSet<ListHistoryEntry> ListHistory => Set<ListHistoryEntry>();
    public DbSet<AuthSession> AuthSessions => Set<AuthSession>();
    public DbSet<AuthTransaction> AuthTransactions => Set<AuthTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.SteamId).IsUnique();
            entity.Property(x => x.SteamId).HasMaxLength(32).IsRequired();
            entity.Property(x => x.Role).HasMaxLength(32).IsRequired();
        });

        modelBuilder.Entity<UserList>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => new { x.UserId, x.Name }).IsUnique();
            entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Visibility).HasConversion<string>().HasMaxLength(16);
            entity
                .HasOne(x => x.User)
                .WithMany(x => x.Lists)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ListItem>(entity =>
        {
            entity.HasKey(x => new { x.ListId, x.GameId });
            entity
                .HasIndex(x => new
                {
                    x.ListId,
                    x.AddedAt,
                    x.GameId,
                })
                .IsDescending(false, true, false);
            entity
                .HasOne(x => x.List)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.ListId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(x => x.GameId);
        });

        modelBuilder.Entity<ListHistoryEntry>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity
                .HasIndex(x => new
                {
                    x.UserId,
                    x.CreatedAt,
                    x.Id,
                })
                .IsDescending(false, true, true);
            entity.Property(x => x.Action).HasConversion<string>().HasMaxLength(16);
            entity
                .HasOne(x => x.List)
                .WithMany(x => x.History)
                .HasForeignKey(x => x.ListId)
                .OnDelete(DeleteBehavior.Cascade);
            entity
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<AuthSession>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.TokenHash).IsUnique();
            entity.Property(x => x.TokenHash).HasMaxLength(128).IsRequired();
            entity
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AuthTransaction>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.CodeHash).IsUnique();
            entity.Property(x => x.CodeHash).HasMaxLength(128).IsRequired();
            entity
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

public enum ListVisibility
{
    Private,
    Public,
}

public enum ListAction
{
    Added,
    Removed,
}

public sealed class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string SteamId { get; set; } = "";
    public string? SteamName { get; set; }
    public string? AvatarUrl { get; set; }
    public string Role { get; set; } = "user";
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public List<UserList> Lists { get; set; } = [];
}

public sealed class UserList
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public ListVisibility Visibility { get; set; } = ListVisibility.Private;
    public bool IsDefault { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public List<ListItem> Items { get; set; } = [];
    public List<ListHistoryEntry> History { get; set; } = [];
}

public sealed class ListItem
{
    public Guid ListId { get; set; }
    public UserList List { get; set; } = null!;
    public long GameId { get; set; }
    public DateTimeOffset AddedAt { get; set; } = DateTimeOffset.UtcNow;
}

public sealed class ListHistoryEntry
{
    public long Id { get; set; }
    public Guid ListId { get; set; }
    public UserList List { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public long GameId { get; set; }
    public ListAction Action { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public sealed class AuthSession
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string TokenHash { get; set; } = "";
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? RevokedAt { get; set; }
}

public sealed class AuthTransaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? UserId { get; set; }
    public User? User { get; set; }
    public string CodeHash { get; set; } = "";
    public string? State { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset? ConsumedAt { get; set; }
}
