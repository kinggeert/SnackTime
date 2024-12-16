using Microsoft.EntityFrameworkCore;
using SnackTime.Models;

namespace SnackTime.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Addon> Addons { get; init; }
    public DbSet<Basket> Baskets { get; init; }
    public DbSet<Discount> Discounts { get; init; }
    public DbSet<Order> Orders { get; init; }
    public DbSet<Product> Products { get; init; }
    public DbSet<ProductCategory> ProductCategories { get; init; }
    public DbSet<ProductCount> ProductCounts { get; init; }
    public DbSet<Role> Roles { get; init; }
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
            entity.HasMany(e => e.ApplicableProducts)
                .WithMany(e => e.AvailableAddons);
            entity.HasMany(e => e.UnavailableWith);
            entity.HasMany(e => e.UsedInProductCounts)
                .WithMany(e => e.AddonsUsed);
        });

        modelBuilder.Entity<Basket>(entity =>
        {
            entity.HasKey(e => e.Identifier);
            entity.HasMany(e => e.Products);
        });
            

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Identifier);
            entity.HasMany(e => e.ApplicableProducts)
                .WithMany(e => e.Discounts);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Identifier);
            entity.HasOne(e => e.Owner)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.OwnerIdentifier);
        });
            

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Identifier);
            entity.HasMany(e => e.AvailableAddons);
            entity.HasOne(e => e.ProductCategory)
                .WithMany(e => e.ProductsInCategory);
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.Identifier);
            entity.HasMany(e => e.ProductsInCategory)
                .WithOne(e => e.ProductCategory);
        });

        modelBuilder.Entity<ProductCount>(entity =>
        {
            entity.HasKey(e => e.Identifier);
            entity.HasOne(e => e.Product);
            entity.HasMany(e => e.AddonsUsed)
                .WithMany(e => e.UsedInProductCounts);
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
        
        
        var customerRole = new Role() {Name = "customer" };
        var cookRole = new Role() {Name = "cook" };
        var adminRole = new Role() {Name = "admin" };
        
        modelBuilder.Entity<Role>().HasData(customerRole);
        modelBuilder.Entity<Role>().HasData(cookRole);
        modelBuilder.Entity<Role>().HasData(adminRole);
    }
}







