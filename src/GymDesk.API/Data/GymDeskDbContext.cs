using GymDesk.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GymDesk.API.Data;

public class GymDeskDbContext : DbContext
{
    public GymDeskDbContext(DbContextOptions<GymDeskDbContext> options)
        : base(options) { }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<TrainingSession> TrainingSessions => Set<TrainingSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subscription>()
            .HasOne(s => s.Client)
            .WithMany()
            .HasForeignKey(s => s.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TrainingSession>()
            .HasOne(ts => ts.Client)
            .WithMany()
            .HasForeignKey(ts => ts.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TrainingSession>()
            .HasOne(ts => ts.Trainer)
            .WithMany()
            .HasForeignKey(ts => ts.TrainerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Subscription>()
            .Property(s => s.Type)
            .HasMaxLength(50);
    }
}