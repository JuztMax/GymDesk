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
        // 1. Настройка имен таблиц и колонок (исправлено под реальную схему БД)

        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("clients");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName).HasColumnName("firstname");
            entity.Property(e => e.LastName).HasColumnName("lastname");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.RegistrationDate).HasColumnName("registrationdate");
        });

        modelBuilder.Entity<Trainer>(entity =>
        {
            entity.ToTable("trainers");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName).HasColumnName("firstname");
            entity.Property(e => e.LastName).HasColumnName("lastname");
            entity.Property(e => e.Specialization).HasColumnName("specialization");
            entity.Property(e => e.Phone).HasColumnName("phone");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.ToTable("subscriptions");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("clientid");
            entity.Property(e => e.Type).HasColumnName("type").HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnName("startdate");
            entity.Property(e => e.EndDate).HasColumnName("enddate");
            entity.Property(e => e.Price).HasColumnName("price");
        });

        modelBuilder.Entity<TrainingSession>(entity =>
        {
            entity.ToTable("trainingsessions"); // 👈 ИСПРАВЛЕНО: было training_sessions
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("clientid");
            entity.Property(e => e.TrainerId).HasColumnName("trainerid");
            entity.Property(e => e.SessionDate).HasColumnName("sessiondate");
            entity.Property(e => e.SessionTime).HasColumnName("sessiontime");
            entity.Property(e => e.Notes).HasColumnName("notes");
        });

        // 2. Настройка связей (Foreign Keys) - осталось без изменений

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
    }
}