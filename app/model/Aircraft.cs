namespace RutAirport.model;

/// <summary>
/// Воздушное судно. Определяет физическую вместимость и схему салона.
/// </summary>
public class Aircraft
{
    public Guid Id { get; set; }

    /// <summary>Модель самолета ("Airbus A320").</summary>
    public string ModelName { get; set; } = string.Empty;

    /// <summary>Массив всех физически существующих мест в салоне.</summary>
    public string[] SeatMap { get; set; } = [];

    /// <summary>Общая вместимость.</summary>
    public int TotalSeats { get; set; }

    /// <summary>
    /// Определяет класс обслуживания для конкретного места на основе конфигурации фюзеляжа.
    /// </summary>
    public ServiceClass GetSeatClass(string seatNumber)
    {
        var rowStr = new string(seatNumber.TakeWhile(char.IsDigit).ToArray());
        if (!int.TryParse(rowStr, out var row))
            return ServiceClass.Economy;

        if (TotalSeats > 200)
        {
            if (row <= 2)
                return ServiceClass.First;
            if (row <= 8)
                return ServiceClass.Business;
            return ServiceClass.Economy;
        }

        if (row <= 3)
            return ServiceClass.Business;

        return ServiceClass.Economy;
    }

    /// <summary>
    /// Возвращает экономический множитель стоимости для класса обслуживания.
    /// </summary>
    public decimal GetMultiplier(ServiceClass serviceClass) => serviceClass switch
    {
        ServiceClass.Economy => 1.0m,
        ServiceClass.Business => 3.0m,
        ServiceClass.First => 5.0m,
        _ => 1.0m
    };
}