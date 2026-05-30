namespace AirportApp.model;

/// <summary>
/// Рейс между аэропортами. Содержит маршрут, самолет, время вылета и коммерческие параметры.
/// </summary>
public class Flight
{
    /// <summary>Уникальный идентификатор рейса.</summary>
    public Guid Id { get; set; }

    /// <summary>Номер рейса.</summary>
    public string FlightNumber { get; set; } = string.Empty;

    /// <summary>Идентификатор аэропорта отправления.</summary>
    public Guid OriginAirportId { get; set; }

    /// <summary>Навигационное свойство к аэропорту отправления.</summary>
    public Airport? OriginAirport { get; set; }

    /// <summary>Идентификатор аэропорта назначения.</summary>
    public Guid DestinationAirportId { get; set; }

    /// <summary>Навигационное свойство к аэропорту назначения.</summary>
    public Airport? DestinationAirport { get; set; }

    /// <summary>Идентификатор выхода на посадку для вылета.</summary>
    public Guid? DepartureGateId { get; set; }

    /// <summary>Навигационное свойство к выходу на посадку.</summary>
    public Gate? DepartureGate { get; set; }

    /// <summary>Идентификатор воздушного судна, выполняющего рейс.</summary>
    public Guid AircraftId { get; set; }

    /// <summary>Навигационное свойство к воздушному судну.</summary>
    public Aircraft? Aircraft { get; set; }
    
    /// <summary>Время вылета в формате UTC.</summary>
    public DateTime DepartureTimeUtc { get; set; }
    
    /// <summary>Количество доступных мест на рейсе.</summary>
    public int AvailableSeats { get; set; } 

    /// <summary>Текущий статус рейса.</summary>
    public FlightStatus Status { get; set; } = FlightStatus.Scheduled;

    /// <summary>Базовая стоимость билета без учета класса обслуживания и скидок.</summary>
    public decimal BasePrice { get; set; }
}
