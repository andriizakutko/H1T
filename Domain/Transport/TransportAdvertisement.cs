using Domain.Abstract;

namespace Domain.Transport;

public class TransportAdvertisement : Advertisement
{
    public virtual TransportType Type { get; set; } // Тип транспорта
    public virtual TransportMake Make { get; set; } // Марка авто
    public virtual TransportModel Model { get; set; } // Модель
    public double EngineCapacity { get; set; } // Объем двигателя
    public string SerialNumber { get; set; } // Серийный номер
    public double FuelConsumption { get; set; } // Расход двигателя
    public string Country { get; set; } // Страна продажи
    public string City { get; set; } // Город продажи
    public string Address { get; set; } // Адрес
    public int Mileage { get; set; } // Пробег
    public string ManufactureCountry { get; set; } // Страна проихводитель
    public DateTime ManufactureDate { get; set; } // Дата выпуска авто
    public bool IsElectric { get; set; }
    public bool IsNew { get; set; }
    public bool IsUsed { get; set; }
    public bool IsVerified { get; set; }
    public virtual ICollection<TransportAdvertisementImage> Images { get; set; }
}