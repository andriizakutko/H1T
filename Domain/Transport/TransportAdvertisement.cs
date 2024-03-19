using Domain.Abstract;

namespace Domain.Transport;

public class TransportAdvertisement : Advertisement
{
    public double EngineCapacity { get; set; }
    public string SerialNumber { get; set; }
    public double FuelConsumption { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public int Mileage { get; set; }
    public string ManufactureCountry { get; set; }
    public DateTime ManufactureDate { get; set; }
    public bool IsElectric { get; set; }
    public bool IsNew { get; set; }
    public bool IsUsed { get; set; }
    public bool IsVerified { get; set; }
    public virtual TransportType Type { get; set; }
    public virtual TransportMake Make { get; set; }
    public virtual TransportModel Model { get; set; }
    public virtual TransportBodyType BodyType { get; set; }
    public virtual ModeratorOverviewStatus ModeratorOverviewStatus { get; set; }
    public virtual ICollection<TransportAdvertisementImage> Images { get; set; }
}