namespace LabCarsData.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;
    public class CarDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder
                    .UseSqlServer(DataSettings.ConnectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Make>(make =>
                {
                    make
                    .HasIndex(m => m.Name)
                    .IsUnique();

                    make
                     .HasMany(m => m.Models)
                     .WithOne(m => m.Make)
                     .HasForeignKey(m => m.MakeId)
                     .OnDelete(DeleteBehavior.Restrict);
                });

            builder
                .Entity<Car>(car =>
                {
                    car
                    .HasIndex(c => c.VIN)
                    .IsUnique();

                    car
                     .HasOne(c => c.Model)
                     .WithMany(m => m.Cars)
                     .HasForeignKey(m => m.ModelId)
                     .OnDelete(DeleteBehavior.Restrict);
                });
        }
    }
}
