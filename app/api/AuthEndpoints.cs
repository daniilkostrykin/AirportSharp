using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RutAirport.database;
using RutAirport.model;

namespace RutAirport.api;

public record LoginRequest(string Username, string Password);
public record RegisterRequest(string Username, string Password); 

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this RouteGroupBuilder api, IConfiguration configuration)
    {
        var group = api.MapGroup("/auth").WithTags("Auth");
        var hasher = new PasswordHasher<User>();

        
        group.MapPost("/register", async (RegisterRequest request, AirportDbContext db) =>
        {
            var exists = await db.Users.AnyAsync(u => u.Username.ToLower() == request.Username.ToLower());
            if (exists) 
                return Results.BadRequest(new { message = "Пользователь с таким логином уже существует" });

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Role = "User" 
            };
            
            
            user.PasswordHash = hasher.HashPassword(user, request.Password);

            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Results.Ok(new { message = "Регистрация успешна. Вам назначена роль: User" });
        })
        .WithSummary("Регистрация нового пользователя (роль User)");

        
        group.MapPost("/login", async (LoginRequest request, AirportDbContext db) =>
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == request.Username.ToLower());
            if (user == null) 
                return Results.Unauthorized();

            
            var verificationResult = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (verificationResult == PasswordVerificationResult.Failed)
                return Results.Unauthorized();

            
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Role, user.Role) 
            };

            var key = Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(configuration["JwtSettings:ExpiryInMinutes"]!)),
                Issuer = configuration["JwtSettings:Issuer"],
                Audience = configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return Results.Ok(new { token = tokenHandler.WriteToken(token) });
        })
        .WithSummary("Вход в систему и получение JWT-токена");

        return api;
    }
}