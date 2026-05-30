namespace AirportApp.model;

/// <summary>
/// Пользователь системы управления аэропортом.
/// </summary>
public class User
{
    /// <summary>Уникальный идентификатор пользователя.</summary>
    public Guid Id { get; set; }

    /// <summary>Имя пользователя для входа в систему.</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>Хэш пароля пользователя.</summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>Роль пользователя в системе. Допустимые значения: Admin, Manager.</summary>
    public string Role { get; set; } = "Manager"; // Роли: Admin, Manager
}
