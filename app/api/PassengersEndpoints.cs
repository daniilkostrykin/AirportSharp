using RutAirport.dto;
using RutAirport.dto.request;
using RutAirport.interfaces;

namespace RutAirport.api;

/// <summary>
/// Регистрирует эндпоинты работы со списком клиентов аэропорта.
/// </summary>
public static class PassengersEndpoints
{
    public static RouteGroupBuilder MapPassengersEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/passengers").WithTags("Passengers");

        
        group.MapGet("/", async (IPassengerService passengers, IMapper mapper) =>
        {
            var result = await passengers.GetAllAsync();
            return Results.Ok(result.Select(mapper.Map));
        })
        .WithSummary("Список всех пассажиров");

        
        group.MapPost("/", async (CreatePassengerRequest request, IPassengerService passengers, IMapper mapper) =>
        {
            var passenger = await passengers.AddAsync(request);
            return Results.Created($"/api/passengers/{passenger.Id}", mapper.Map(passenger));
        })
        .WithSummary("Зарегистрировать нового пассажира в базе");

        return api;
    }
}
