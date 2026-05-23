namespace RutAirport.model;

/// <summary>
/// Рейс аэропорта. Содержит информацию о маршруте, времени и доступных местах.
/// </summary>
public class Flight
{
    /// <summary>
    /// Уникальный идентификатор рейса.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Номер рейса (например, "SU-101").
    /// </summary>
    public string FlightNumber { get; set; } = string.Empty;

    /// <summary>
    /// Пункт назначения.
    /// </summary>
    public string Destination { get; set; } = string.Empty;

    /// <summary>
    /// Время вылета в формате UTC (всемирное координированное время).
    /// </summary>
    public DateTime DepartureTimeUtc { get; set; }
    
    /// <summary>
    /// Массив всех физически существующих мест в салоне самолета (например, ["1A", "1B"]).
    /// </summary>
    public string[] AllSeats { get; set; } = []; 
    
    /// <summary>
    /// Общая вместимость самолета.
    /// </summary>
    public int TotalSeats { get; set; }

    /// <summary>
    /// Количество оставшихся свободных мест для бронирования.
    /// </summary>
    public int AvailableSeats { get; set; }
    
    /// <summary>
    /// Текущий статус рейса (по расписанию, посадка, вылетел, отменен).
    /// </summary>
    public FlightStatus Status { get; set; } = FlightStatus.Scheduled;

    /// <summary>
    /// Страна вылета.
    /// </summary>
    public string OriginCountry { get; set; } = string.Empty;

    /// <summary>
    /// Страна прибытия.
    /// </summary>
    public string DestinationCountry { get; set; } = string.Empty;

    /// <summary>
    /// Базовая стоимость билета в рублях.
    /// </summary>
    public decimal BasePrice { get; set; }
}