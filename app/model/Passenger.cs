namespace AirportApp.model;

/// <summary>
/// Пассажир аэропорта. Содержит личные данные и статус лояльности.
/// </summary>
public class Passenger
{
    /// <summary>
    /// Уникальный идентификатор пассажира.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Полное имя пассажира.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Серия и номер паспорта или другого документа, удостоверяющего личность.
    /// </summary>
    public string PassportNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Признак VIP-клиента. Позволяет использовать автоназначение виртуальных мест при овербукинге.
    /// </summary>
    public bool IsVip { get; set; } 
}
