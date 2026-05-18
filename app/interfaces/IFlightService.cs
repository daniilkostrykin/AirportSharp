using RutAirport.dto.request;
using RutAirport.model;

namespace RutAirport.interfaces;

/// <summary>
/// Интерфейс сервиса управления рейсами.
/// </summary>
public interface IFlightService
{
    /// <summary>
    /// Получить список всех рейсов, отсортированных по времени вылета.
    /// </summary>
    Task<IReadOnlyList<Flight>> GetAllAsync();

    /// <summary>
    /// Добавить новый рейс в расписание.
    /// </summary>
    Task<Flight> AddAsync(CreateFlightRequest request);

    /// <summary>
    /// Изменить текущий статус рейса.
    /// </summary>
    Task<Flight?> ChangeStatusAsync(Guid id, FlightStatus newStatus);
}