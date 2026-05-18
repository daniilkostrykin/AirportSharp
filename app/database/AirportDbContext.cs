using Microsoft.EntityFrameworkCore;
using RutAirport.model;

namespace RutAirport.database;

/// <summary>
/// Контекст базы данных аэропорта.
/// Описывает правила создания таблиц в PostgreSQL.
/// </summary>
public class AirportDbContext(DbContextOptions<AirportDbContext> options) : DbContext(options)
{
    public DbSet<Flight> Flights { get; set; }
    public DbSet<Passenger> Passengers { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.FlightNumber).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Destination).HasMaxLength(100).IsRequired();
        });

        
        modelBuilder.Entity<Passenger>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.FullName).HasMaxLength(200).IsRequired();
            entity.Property(x => x.PassportNumber).HasMaxLength(50).IsRequired();
        });

        
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.SeatNumber).HasMaxLength(10).IsRequired();
        });
    }
}
