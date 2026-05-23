namespace RutAirport.model;

/// <summary>
/// Выход на посадку. Привязан к конкретному аэропорту.
/// </summary>
public class Gate
{
    public Guid Id { get; set; }
    
    /// <summary>Внешний ключ для связи с таблицей Airports.</summary>
    public Guid AirportId { get; set; }
    
    /// <summary>Номер выхода ("14A", "B2").</summary>
    public string Name { get; set; } = string.Empty;

    public Airport? Airport { get; set; }
}