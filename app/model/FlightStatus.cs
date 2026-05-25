using System.Text.Json.Serialization;

namespace AirportApp.model;

/// <summary>
/// Перечисление возможных статусов рейса.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FlightStatus
{
    /// <summary>
    /// Рейс по расписанию.
    /// </summary>
    Scheduled, 

    /// <summary>
    /// Идет посадка на самолет.
    /// </summary>
    Boarding,  

    /// <summary>
    /// Самолет вылетел из аэропорта.
    /// </summary>
    Departed,  

    /// <summary>
    /// Рейс отменен.
    /// </summary>
    Cancelled  
}
