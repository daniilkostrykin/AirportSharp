using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using RutAirport.api;
using RutAirport.database;
using RutAirport.dto;
using RutAirport.interfaces;
using RutAirport.services;
using RutAirport.model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "RUT Airport API", Version = "v1" });
});

var connectionString = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddDbContext<AirportDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IPassengerService, PassengerService>();
builder.Services.AddScoped<ICheckInService, CheckInService>();
builder.Services.AddScoped<IMapper, Mapper>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AirportDbContext>();
        await db.Database.EnsureCreatedAsync(); 

        
        if (!db.Airports.Any()) 
        {
            
            var svo = new Airport { Id = Guid.NewGuid(), IataCode = "SVO", Name = "Шереметьево", City = "Москва", Country = "Россия", TimeZone = "Europe/Moscow" };
            var kzn = new Airport { Id = Guid.NewGuid(), IataCode = "KZN", Name = "Казань", City = "Казань", Country = "Россия", TimeZone = "Europe/Moscow" };
            var vvo = new Airport { Id = Guid.NewGuid(), IataCode = "VVO", Name = "Кневичи", City = "Владивосток", Country = "Россия", TimeZone = "Asia/Vladivostok" };
            var ovb = new Airport { Id = Guid.NewGuid(), IataCode = "OVB", Name = "Толмачево", City = "Новосибирск", Country = "Россия", TimeZone = "Asia/Novosibirsk" };
            var svx = new Airport { Id = Guid.NewGuid(), IataCode = "SVX", Name = "Кольцово", City = "Екатеринбург", Country = "Россия", TimeZone = "Asia/Yekaterinburg" };
            
            db.Airports.AddRange(svo, kzn, vvo, ovb, svx);

            
            var gate1 = new Gate { Id = Guid.NewGuid(), AirportId = svo.Id, Name = "14A" };
            var gate2 = new Gate { Id = Guid.NewGuid(), AirportId = svo.Id, Name = "14B" };
            var gate3 = new Gate { Id = Guid.NewGuid(), AirportId = svo.Id, Name = "15" };
            db.Gates.AddRange(gate1, gate2, gate3);

            
            var seatsList = new List<string>();
            for (int row = 1; row <= 5; row++)
            {
                foreach (var letter in new[] { "A", "B", "C", "D", "E", "F" })
                {
                    seatsList.Add($"{row}{letter}");
                }
            }
            var seats = seatsList.ToArray();

            
            var flights = new List<Flight>
            {
                new() { Id = Guid.NewGuid(), FlightNumber = "SU-101", OriginAirportId = svo.Id, DestinationAirportId = kzn.Id, DepartureGateId = gate1.Id, DepartureTimeUtc = DateTime.UtcNow.AddDays(1), BasePrice = 5500, AllSeats = seats, TotalSeats = seats.Length, AvailableSeats = seats.Length, Status = FlightStatus.Scheduled },
                new() { Id = Guid.NewGuid(), FlightNumber = "RT-202", OriginAirportId = svo.Id, DestinationAirportId = vvo.Id, DepartureGateId = gate2.Id, DepartureTimeUtc = DateTime.UtcNow.AddDays(2), BasePrice = 25000, AllSeats = seats, TotalSeats = seats.Length, AvailableSeats = seats.Length, Status = FlightStatus.Scheduled },
                new() { Id = Guid.NewGuid(), FlightNumber = "ZX-999", OriginAirportId = svo.Id, DestinationAirportId = ovb.Id, DepartureGateId = gate3.Id, DepartureTimeUtc = DateTime.UtcNow.AddDays(3), BasePrice = 12000, AllSeats = seats, TotalSeats = seats.Length, AvailableSeats = seats.Length, Status = FlightStatus.Scheduled },
                new() { Id = Guid.NewGuid(), FlightNumber = "S7-777", OriginAirportId = svo.Id, DestinationAirportId = svx.Id, DepartureGateId = null,       DepartureTimeUtc = DateTime.UtcNow.AddHours(12), BasePrice = 8000, AllSeats = seats, TotalSeats = seats.Length, AvailableSeats = seats.Length, Status = FlightStatus.Boarding },
                
                new() { Id = Guid.NewGuid(), FlightNumber = "U6-333", OriginAirportId = svx.Id, DestinationAirportId = svo.Id, DepartureGateId = null,       DepartureTimeUtc = DateTime.UtcNow.AddDays(5), BasePrice = 7500, AllSeats = seats, TotalSeats = seats.Length, AvailableSeats = seats.Length, Status = FlightStatus.Scheduled }
            };
            db.Flights.AddRange(flights);

            
            var passengers = new List<Passenger>
            {
                new() { Id = Guid.NewGuid(), FullName = "Даня", PassportNumber = "7777 000001", IsVip = true },
                new() { Id = Guid.NewGuid(), FullName = "Дима", PassportNumber = "1234 567890", IsVip = false },    
                new() { Id = Guid.NewGuid(), FullName = "Алиса", PassportNumber = "0987 654321", IsVip = false },
                new() { Id = Guid.NewGuid(), FullName = "Маша", PassportNumber = "1111 222222", IsVip = true },
                new() { Id = Guid.NewGuid(), FullName = "Петя", PassportNumber = "3333 444444", IsVip = false },
                new() { Id = Guid.NewGuid(), FullName = "Катя", PassportNumber = "5555 666666", IsVip = false },
                new() { Id = Guid.NewGuid(), FullName = "Саня", PassportNumber = "7777 888888", IsVip = true },
                new() { Id = Guid.NewGuid(), FullName = "Оля", PassportNumber = "9999 000000", IsVip = false },
                new() { Id = Guid.NewGuid(), FullName = "Игорь", PassportNumber = "1010 202020", IsVip = false },
                new() { Id = Guid.NewGuid(), FullName = "Света", PassportNumber = "3030 404040", IsVip = false }
            };
            db.Passengers.AddRange(passengers);

            await db.SaveChangesAsync();
        }
    }

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "RUT Airport API v1");
        options.RoutePrefix = "swagger";
    });
}

var api = app.MapGroup("/api");
api.MapFlightsEndpoints();
api.MapPassengersEndpoints();
api.MapCheckInEndpoints();

app.MapGet("/", () => Results.Ok(new { message = "Аэропорт работает! Перейдите на /swagger" }));

await app.RunAsync();