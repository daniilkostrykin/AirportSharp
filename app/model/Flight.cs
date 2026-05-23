namespace RutAirport.model;

public class Flight
{
    public Guid Id { get; set; }
    public string FlightNumber { get; set; } = string.Empty;

    public Guid OriginAirportId { get; set; }
    public Airport? OriginAirport { get; set; }

    public Guid DestinationAirportId { get; set; }
    public Airport? DestinationAirport { get; set; }

    public Guid? DepartureGateId { get; set; }
    public Gate? DepartureGate { get; set; }

    public Guid AircraftId { get; set; }
    public Aircraft? Aircraft { get; set; }
    
    public DateTime DepartureTimeUtc { get; set; }
    
    public int AvailableSeats { get; set; } 
    public FlightStatus Status { get; set; } = FlightStatus.Scheduled;
    public decimal BasePrice { get; set; }
}