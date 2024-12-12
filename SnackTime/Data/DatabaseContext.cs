﻿using Microsoft.EntityFrameworkCore;
using SnackTime.Models;

namespace SnackTime.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Addon> Addons { get; init; }
    public DbSet<Basket> Baskets { get; init; }
    public DbSet<Discount> Discounts { get; init; }
    public DbSet<Order> Orders { get; init; }
    public DbSet<Product> Products { get; init; }
    public DbSet<ProductCount> ProductCounts { get; init; }
    public DbSet<User> Users { get; init; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connection =
            @"Data Source=.;Initial Catalog=SnackTime;Integrated Security=true;TrustServerCertificate=true;";

        optionsBuilder.UseSqlServer(connection);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Addon>(entity =>
        {
            entity.HasKey(e => e.Identifier);
            entity.HasMany(addon => addon.ApplicableProducts)
                .WithMany(product => product.AvailableAddons);
            entity.HasMany(addon => addon.UnavailableWith);
        });

        modelBuilder.Entity<Basket>(entity =>
        {
            entity.HasKey(e => e.Identifier);
            entity.HasMany(e => e.Products);
            entity.HasOne(e => e.Owner)
                .WithOne(e => e.Basket)
                .HasForeignKey<User>(e => e.BasketIdentifier);
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Identifier);
            entity.HasMany(e => e.ApplicableProducts)
                .WithMany(e => e.Discounts);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasMany(e => e.Products);
            entity.HasOne(e => e.Owner);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Identifier);
            entity.HasMany(e => e.AvailableAddons);
        });

        modelBuilder.Entity<ProductCount>(entity =>
        {
            entity.HasKey(e => e.Identifier);
            entity.HasOne(e => e.Product);
            entity.HasMany(e => e.AddonsUsed);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Name);
            entity.HasMany(e => e.UsersWithRole);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Identifier);
            entity.HasOne(e => e.Role);
            entity.HasOne(e => e.Basket);
            entity.HasMany(e => e.Orders);
        });
    }
}






