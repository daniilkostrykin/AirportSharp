using Microsoft.EntityFrameworkCore;
using RutAirport.model;

namespace RutAirport.database;

public class AirportDbContext(DbContextOptions<AirportDbContext> options) : DbContext(options)
{
    public DbSet<Flight> Flights { get; set; }
    public DbSet<Passenger> Passengers { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Airport> Airports { get; set; }
    public DbSet<Gate> Gates { get; set; }
    public DbSet<Aircraft> Aircrafts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Airport>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.IataCode).HasMaxLength(3).IsRequired();
            entity.Property(x => x.City).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Gate>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(10).IsRequired();
            entity.HasOne(g => g.Airport).WithMany(a => a.Gates).HasForeignKey(g => g.AirportId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Aircraft>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.ModelName).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.FlightNumber).HasMaxLength(20).IsRequired();
            
            
            entity.HasOne(f => f.OriginAirport)
                  .WithMany()
                  .HasForeignKey(f => f.OriginAirportId)
                  .OnDelete(DeleteBehavior.Restrict); 

            
            entity.HasOne(f => f.DestinationAirport)
                  .WithMany()
                  .HasForeignKey(f => f.DestinationAirportId)
                  .OnDelete(DeleteBehavior.Restrict);

            
            entity.HasOne(f => f.DepartureGate)
                  .WithMany()
                  .HasForeignKey(f => f.DepartureGateId)
                  .OnDelete(DeleteBehavior.SetNull); 

            entity.HasOne(f => f.Aircraft)
              .WithMany()
              .HasForeignKey(f => f.AircraftId)
              .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Passenger>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.FullName).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.SeatNumber).HasMaxLength(10);
        });
    }
}