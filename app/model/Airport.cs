namespace AirportApp.model;

/// <summary>
/// Аэропорт. Содержит географические данные и международный код.
/// </summary>
public class Airport
{
    public Guid Id { get; set; }
    
    /// <summary>Трехбуквенный код IATA (SVO, DME, JFK).</summary>
    public string IataCode { get; set; } = string.Empty; 
    
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    
    /// <summary>Временная зона ("Europe/Moscow").</summary>
    public string TimeZone { get; set; } = string.Empty;

    
    public List<Gate> Gates { get; set; } = [];
}
