namespace AirportApp.model;

/// <summary>
/// Выход на посадку. Привязан к конкретному аэропорту.
/// </summary>
public class Gate
{
    /// <summary>Уникальный идентификатор выхода на посадку.</summary>
    public Guid Id { get; set; }
    
    /// <summary>Внешний ключ для связи с таблицей Airports.</summary>
    public Guid AirportId { get; set; }
    
    /// <summary>Номер выхода ("14A", "B2").</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Навигационное свойство к аэропорту, которому принадлежит выход.</summary>
    public Airport? Airport { get; set; }
}
