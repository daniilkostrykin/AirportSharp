using RutAirport.dto;
using RutAirport.dto.request;
using RutAirport.dto.response;
using RutAirport.interfaces;
using RutAirport.model;

namespace RutAirport.api;

/// <summary>
/// Маршруты API для управления рейсами аэропорта.
/// </summary>
public static class FlightsEndpoints
{
    /// <summary>
    /// Регистрирует эндпоинты работы с расписанием и статусами рейсов.
    /// </summary>
    public static RouteGroupBuilder MapFlightsEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/flights").WithTags("Flights");

        
        group.MapGet("/", async (IFlightService flights, IMapper mapper) =>
        {
            var result = await flights.GetAllAsync();
            return Results.Ok(result.Select(mapper.Map));
        })
        .WithSummary("Табло рейсов");

        
        group.MapPost("/", async (CreateFlightRequest request, IFlightService flights, IMapper mapper) =>
        {
            var flight = await flights.AddAsync(request);
            return Results.Created($"/api/flights/{flight.Id}", mapper.Map(flight));
        })
        .WithSummary("Добавить новый рейс");

        
        group.MapPut("/{id:guid}/status", async (Guid id, FlightStatus status, IFlightService flights, IMapper mapper) =>
        {
            var flight = await flights.ChangeStatusAsync(id, status);
            return flight == null 
                ? Results.NotFound(new ErrorResponse("Рейс не найден.")) 
                : Results.Ok(mapper.Map(flight));
        })
        .WithSummary("Изменить статус рейса (например, начать посадку)");

        
        group.MapGet("/{id:guid}", async (Guid id, IFlightService flights, IMapper mapper) =>
        {
            var flight = await flights.GetByIdAsync(id);
            return flight == null 
                ? Results.NotFound(new ErrorResponse("Рейс не найден.")) 
                : Results.Ok(mapper.Map(flight));
        })
        .WithSummary("Получить информацию о конкретном рейсе");
        return api;
    }
}
