using Microsoft.EntityFrameworkCore;
using CarAPI.Models;

namespace CarAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<CarBrand> CarBrands { get; set; }
        public DbSet<TrimLevel> TrimLevels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.CarBrand)
                .WithMany(b => b.Cars)
                .HasForeignKey(c => c.CarBrandId);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.TrimLevel)
                .WithMany(t => t.Cars)
                .HasForeignKey(c => c.TrimLevelId);

            modelBuilder.Entity<CarBrand>().HasData(
                new CarBrand { Id = 1, Name = "Toyota" },
                new CarBrand { Id = 2, Name = "BMW" },
                new CarBrand { Id = 3, Name = "Mercedes" }
            );

            modelBuilder.Entity<TrimLevel>().HasData(
                new TrimLevel { Id = 1, Name = "Standard" },
                new TrimLevel { Id = 2, Name = "Comfort" },
                new TrimLevel { Id = 3, Name = "Premium" }
            );

            modelBuilder.Entity<Car>().HasData(
                new Car { Id = 1, Name = "Camry", CarBrandId = 1, TrimLevelId = 2 },
                new Car { Id = 2, Name = "X5", CarBrandId = 2, TrimLevelId = 3 },
                new Car { Id = 3, Name = "E-Class", CarBrandId = 3, TrimLevelId = 1 }
            );
        }
    }
}