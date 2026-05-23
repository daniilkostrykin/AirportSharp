namespace RutAirport.model;

/// <summary>
/// Воздушное судно. Определяет физическую вместимость и схему салона.
/// </summary>
public class Aircraft
{
    public Guid Id { get; set; }
    
    /// <summary>Модель самолета ("Airbus A320").</summary>
    public string ModelName { get; set; } = string.Empty;
    
    /// <summary>Массив всех физически существующих мест в салоне.</summary>
    public string[] SeatMap { get; set; } = [];
    
    /// <summary>Общая вместимость.</summary>
    public int TotalSeats { get; set; }
}