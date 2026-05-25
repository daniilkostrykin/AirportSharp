using AirportApp.dto;
using AirportApp.dto.request;
using AirportApp.interfaces;

namespace AirportApp.api;

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
        .WithSummary("Список всех пассажиров (Только для персонала)")
        .RequireAuthorization(policy => policy.RequireRole("Admin", "Manager"));

        
        group.MapPost("/", async (CreatePassengerRequest request, IPassengerService passengers, IMapper mapper) =>
        {
            var passenger = await passengers.AddAsync(request);
            return Results.Created($"/api/passengers/{passenger.Id}", mapper.Map(passenger));
        })
        .WithSummary("Зарегистрировать профиль пассажира")
        .RequireAuthorization();

        return api;
    }
}
