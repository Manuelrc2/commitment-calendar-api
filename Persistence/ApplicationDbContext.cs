using commitment_calendar_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace commitment_calendar_api.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                // NECESSARY CONFIGURATIONS
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Description)
                    .HasMaxLength(1000);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Stake)
                    .HasPrecision(18, 2);

                entity.HasIndex(e => new { e.UserId, e.StartsAt })
                    .IncludeProperties(e => new { e.Name, e.Description, e.EndsAt, e.Stake });

                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.StartsAt);
            });
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}