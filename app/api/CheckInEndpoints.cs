using RutAirport.dto;
using RutAirport.dto.request;
using RutAirport.dto.response;
using RutAirport.interfaces;

namespace RutAirport.api;

/// <summary>
/// Маршруты API для регистрации на рейсы и отмены посадочных талонов.
/// </summary>
public static class CheckInEndpoints
{
    /// <summary>
    /// Регистрирует эндпоинты управления посадкой.
    /// </summary>
    public static RouteGroupBuilder MapCheckInEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/checkin").WithTags("Check-In");

        
        group.MapPost("/", async (CheckInRequest request, ICheckInService checkIn, IMapper mapper) =>
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
        .WithSummary("Зарегистрировать пассажира на конкретное место");

        
        group.MapDelete("/{ticketId:guid}", async (Guid ticketId, ICheckInService checkIn) =>
        {
            var result = await checkIn.CancelCheckInAsync(ticketId);
            return result 
                ? Results.Ok(new { message = "Регистрация успешно отменена, место возвращено." }) 
                : Results.NotFound(new ErrorResponse("Талон не найден."));
        })
        .WithSummary("Отменить регистрацию и вернуть место");

        return api;
    }
}
