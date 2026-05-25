using AirportApp.dto;
using AirportApp.dto.request;
using AirportApp.dto.response;
using AirportApp.interfaces;

namespace AirportApp.api;

public static class CheckInEndpoints
{
    public static RouteGroupBuilder MapCheckInEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/tickets").WithTags("Tickets & Check-In");

        
        group.MapPost("/buy", async (BuyTicketRequest request, ICheckInService checkIn, IMapper mapper) =>
        {
            try
            {
                var ticket = await checkIn.BuyTicketAsync(request);
                return Results.Ok(mapper.Map(ticket));
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
            {
                return Results.BadRequest(new ErrorResponse(ex.Message));
            }
        })
        .WithSummary("1. Купить билет на рейс (без выбора места)")
        .RequireAuthorization(policy => policy.RequireRole("Admin", "Manager", "User"));

        
        group.MapPost("/checkin", async (CheckInRequest request, ICheckInService checkIn, IMapper mapper) =>
        {
            try
            {
                var ticket = await checkIn.RegisterPassengerAsync(request);
                return Results.Ok(mapper.Map(ticket));
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
            {
                return Results.BadRequest(new ErrorResponse(ex.Message));
            }
        })
        .WithSummary("2. Пройти регистрацию (назначить место)")
        .RequireAuthorization(policy => policy.RequireRole("Admin", "Manager", "User"));

        
        group.MapDelete("/checkin/{ticketId:guid}", async (Guid ticketId, ICheckInService checkIn) =>
        {
            var result = await checkIn.CancelCheckInAsync(ticketId);
            return result 
                ? Results.Ok(new { message = "Посадка отменена, место освобождено. Билет активен." }) 
                : Results.NotFound(new ErrorResponse("Билет не найден или регистрация еще не пройдена."));
        })
        .WithSummary("3. Отменить регистрацию (Только для персонала)")
        .RequireAuthorization(policy => policy.RequireRole("Admin", "Manager")); 
        return api;
    }
}
