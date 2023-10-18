using Microsoft.EntityFrameworkCore;
using PaintyTask.Domain.Models;
using PaintyTask.Infrastructure.Context.SeedData;

namespace PaintyTask.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UserData> Users { get; set; }
    public DbSet<FriendshipData> Friendships { get; set; }
    public DbSet<PhotoData> Photos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserData>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Login).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.Password).IsRequired();

            entity.HasMany(e => e.SentFriendshipRequests)
                .WithOne()
                .HasForeignKey(e => e.UserSenderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.ReceivedFriendshipRequests)
                .WithOne()
                .HasForeignKey(e => e.UserReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Photos)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<FriendshipData>(entity =>
        {
            entity.HasKey(f => new { f.UserSenderId, f.UserReceiverId });

            entity.HasOne(e => e.UserSender)
                .WithMany(f => f.SentFriendshipRequests)
                .HasForeignKey(e => e.UserSenderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.UserReceiver)
                .WithMany(f => f.ReceivedFriendshipRequests)
                .HasForeignKey(e => e.UserReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PhotoData>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Url).IsRequired();
            entity.Property(e => e.UserId).IsRequired();

            entity.HasOne(e => e.User)
                .WithMany(u => u.Photos)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.SeedUsers();
        modelBuilder.SeedPhotos();
        modelBuilder.SeedFriendships();
    }
}