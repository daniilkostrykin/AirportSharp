using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Identity;
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

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.ParameterLocation.Header,
        Description = "Введите токен в формате: Bearer {твой_токен}",
        Name = "Authorization",
        Type = Microsoft.OpenApi.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(document =>
    {
        var securitySchemeRef = new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", document, null);
        var requirement = new Microsoft.OpenApi.OpenApiSecurityRequirement();
        requirement.Add(securitySchemeRef, new List<string>());
        return requirement;
    });
});


var connectionString = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddDbContext<AirportDbContext>(options => options.UseNpgsql(connectionString));


var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});

builder.Services.AddAuthorization();

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

        
        if (!db.Users.Any())
        {
            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "danya_admin",
                Role = "Admin" 
            };
            var hasher = new PasswordHasher<User>();
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "rut_miit_2026");
            db.Users.Add(adminUser);
            await db.SaveChangesAsync();
        }

        
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

            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "data", "aircrafts.json");
            var aircraftsJson = await File.ReadAllTextAsync(jsonPath);
            var aircraftSeedData = System.Text.Json.JsonSerializer.Deserialize<List<AircraftSeedDto>>(aircraftsJson);

            var aircraftDict = new Dictionary<string, Aircraft>();
            var alphabet = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            foreach (var dto in aircraftSeedData!)
            {
                var seatMap = new List<string>();
                for (int r = 1; r <= dto.Rows; r++)
                {
                    for (int s = 0; s < dto.SeatsPerRow; s++) 
                    {
                        seatMap.Add($"{r}{alphabet[s]}");
                    }
                }

                var aircraft = new Aircraft 
                { 
                    Id = Guid.NewGuid(), 
                    ModelName = dto.ModelName, 
                    SeatMap = seatMap.ToArray(), 
                    TotalSeats = seatMap.Count 
                };
                db.Aircrafts.Add(aircraft);
                aircraftDict[dto.ModelName] = aircraft; 
            }
            await db.SaveChangesAsync();

            var ssj = aircraftDict["Sukhoi Superjet 100-95B"];
            var b777 = aircraftDict["Boeing 777-300ER"];
            var mc21 = aircraftDict["Irkut MC-21-300"];
            var b737 = aircraftDict["Boeing 737-800"];
            var a350 = aircraftDict["Airbus A350-900"];

            var flights = new List<Flight>
            {
                new() { Id = Guid.NewGuid(), FlightNumber = "SU-101", OriginAirportId = svo.Id, DestinationAirportId = kzn.Id, DepartureGateId = gate1.Id, AircraftId = ssj.Id,  DepartureTimeUtc = DateTime.UtcNow.AddDays(1), BasePrice = 5500,  AvailableSeats = ssj.TotalSeats,  Status = FlightStatus.Scheduled },
                new() { Id = Guid.NewGuid(), FlightNumber = "RT-202", OriginAirportId = svo.Id, DestinationAirportId = vvo.Id, DepartureGateId = gate2.Id, AircraftId = b777.Id, DepartureTimeUtc = DateTime.UtcNow.AddDays(2), BasePrice = 25000, AvailableSeats = b777.TotalSeats, Status = FlightStatus.Scheduled },
                new() { Id = Guid.NewGuid(), FlightNumber = "ZX-999", OriginAirportId = svo.Id, DestinationAirportId = ovb.Id, DepartureGateId = gate3.Id, AircraftId = mc21.Id, DepartureTimeUtc = DateTime.UtcNow.AddDays(3), BasePrice = 12000, AvailableSeats = mc21.TotalSeats, Status = FlightStatus.Scheduled },
                new() { Id = Guid.NewGuid(), FlightNumber = "S7-777", OriginAirportId = svo.Id, DestinationAirportId = svx.Id, DepartureGateId = gate1.Id, AircraftId = b737.Id, DepartureTimeUtc = DateTime.UtcNow.AddHours(12), BasePrice = 8000, AvailableSeats = b737.TotalSeats, Status = FlightStatus.Boarding }, 
                new() { Id = Guid.NewGuid(), FlightNumber = "U6-333", OriginAirportId = svx.Id, DestinationAirportId = svo.Id, DepartureGateId = gate3.Id, AircraftId = a350.Id, DepartureTimeUtc = DateTime.UtcNow.AddDays(5), BasePrice = 7500,  AvailableSeats = a350.TotalSeats, Status = FlightStatus.Scheduled }
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


app.UseAuthentication();
app.UseAuthorization();

var api = app.MapGroup("/api");


api.MapAuthEndpoints(builder.Configuration);

api.MapFlightsEndpoints();
api.MapPassengersEndpoints();
api.MapCheckInEndpoints();

app.MapGet("/", () => Results.Ok(new { message = "Аэропорт работает! Перейдите на /swagger" }));
await app.RunAsync();

public class AircraftSeedDto
{
    public string ModelName { get; set; } = string.Empty;
    public int Rows { get; set; }
    public int SeatsPerRow { get; set; }
}