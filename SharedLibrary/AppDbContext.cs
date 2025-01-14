using Microsoft.Extensions.Configuration;

namespace SharedLibrary;

using Entities;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder() 
                .SetBasePath(AppContext.BaseDirectory) 
                .AddJsonFile("appsettings.json") 
                .Build();
            
            var connectionString = configuration.GetConnectionString("DefaultConnection") 
                                   ?? throw new NullReferenceException("Default connection string");
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reservations)
            .HasForeignKey(r => r.UserId);
        
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Activity)
            .WithMany(a => a.Reservations)
            .HasForeignKey(r => r.ActivityId);

        modelBuilder.Entity<Activity>()
            .HasOne(a => a.Trainer)
            .WithMany(u => u.Activites)
            .HasForeignKey(a => a.TrainerId);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}
