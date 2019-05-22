using Microsoft.EntityFrameworkCore;
using ReproducePostgresIssue.Entities;

namespace ReproducePostgresIssue
{
    public class AntFarmContext : DbContext
    {
        public DbSet<Ant> Ants { get; set; }

        public DbSet<Queen> Queens { get; set; }

        public DbSet<Hive> Hives { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Database=Antfarm;Username=my_user;Password=my_pw");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hive>()
                .HasMany(h => h.Ants)
                .WithOne(w => w.Hive)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Hive>()
                .HasOne(h => h.Queen)
                .WithOne(q => q.Hive)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}