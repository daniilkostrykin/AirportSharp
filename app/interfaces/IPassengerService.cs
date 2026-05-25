using AirportApp.dto.request;
using AirportApp.model;

namespace AirportApp.interfaces;

/// <summary>
/// Интерфейс сервиса управления пассажирами.
/// </summary>
public interface IPassengerService
{
    /// <summary>
    /// Получить список всех зарегистрированных в системе пассажиров.
    /// </summary>
    Task<IReadOnlyList<Passenger>> GetAllAsync();

    /// <summary>
    /// Добавить нового пассажира в систему аэропорта.
    /// </summary>
    Task<Passenger> AddAsync(CreatePassengerRequest request);
}
