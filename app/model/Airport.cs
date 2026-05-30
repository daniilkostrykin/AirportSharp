namespace AirportApp.model;

/// <summary>
/// Аэропорт. Содержит географические данные и международный код.
/// </summary>
public class Airport
{
    /// <summary>Уникальный идентификатор аэропорта.</summary>
    public Guid Id { get; set; }
    
    /// <summary>Трехбуквенный код IATA (SVO, DME, JFK).</summary>
    public string IataCode { get; set; } = string.Empty; 
    
    /// <summary>Название аэропорта.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Город, в котором расположен аэропорт.</summary>
    public string City { get; set; } = string.Empty;

    /// <summary>Страна, в которой расположен аэропорт.</summary>
    public string Country { get; set; } = string.Empty;
    
    /// <summary>Временная зона ("Europe/Moscow").</summary>
    public string TimeZone { get; set; } = string.Empty;

    /// <summary>Список выходов на посадку, доступных в аэропорту.</summary>
    public List<Gate> Gates { get; set; } = [];
}
