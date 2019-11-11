﻿namespace LabCarsData.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;
    public class CarDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<CarPurchase> Purchases { get; set; }

        public DbSet<Address> Addresses { get; set; }

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

            builder
                .Entity<CarPurchase>(purchase =>
                {
                    purchase
                    .HasKey(p => new { p.CustomerId, p.CarId });

                    purchase
                    .HasOne(p => p.Customer)
                    .WithMany(c => c.Purcheses)
                    .HasForeignKey(p => p.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                     purchase
                     .HasOne(p => p.Car)
                     .WithMany(c => c.Owners)
                     .HasForeignKey(p => p.CarId)
                     .OnDelete(DeleteBehavior.Restrict);
                });

            builder
                .Entity<Customer>(customer =>
                {
                    customer
                    .HasOne(c => c.Address)
                    .WithOne(a => a.Customer)
                    .HasForeignKey<Address>(k => k.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
                });

        }   
    }
}
