using Microsoft.EntityFrameworkCore;
using Notifications.Features.ReleaseAlerts.Models;

namespace Notifications.Infrastructure.Persistence;

public sealed class NotificationDbContext(DbContextOptions<NotificationDbContext> options)
    : DbContext(options)
{
    public DbSet<ReleaseAlertEvent> ReleaseAlertEvents => Set<ReleaseAlertEvent>();
    public DbSet<ReleaseAlertPlatform> ReleaseAlertPlatforms => Set<ReleaseAlertPlatform>();
    public DbSet<NotificationDelivery> NotificationDeliveries => Set<NotificationDelivery>();
    public DbSet<NotificationDeliveryAttempt> NotificationDeliveryAttempts =>
        Set<NotificationDeliveryAttempt>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("notifications");

        modelBuilder.Entity<ReleaseAlertEvent>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => new { x.GameId, x.ReleaseDayUtc }).IsUnique();
            entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(16);
        });

        modelBuilder.Entity<ReleaseAlertPlatform>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => new { x.ReleaseAlertEventId, x.SourceReleaseDateId }).IsUnique();
            entity
                .HasOne(x => x.ReleaseAlertEvent)
                .WithMany(x => x.Platforms)
                .HasForeignKey(x => x.ReleaseAlertEventId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<NotificationDelivery>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity
                .HasIndex(x => new
                {
                    x.ReleaseAlertEventId,
                    x.UserId,
                    x.PushEndpointId,
                    x.Provider,
                })
                .IsUnique();
            entity.HasIndex(x => new { x.Status, x.CreatedAtUtc });
            entity.Property(x => x.Provider).HasConversion<string>().HasMaxLength(16);
            entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(16);
            entity
                .HasOne(x => x.ReleaseAlertEvent)
                .WithMany(x => x.Deliveries)
                .HasForeignKey(x => x.ReleaseAlertEventId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<NotificationDeliveryAttempt>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity
                .HasOne(x => x.NotificationDelivery)
                .WithMany(x => x.Attempts)
                .HasForeignKey(x => x.NotificationDeliveryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}
