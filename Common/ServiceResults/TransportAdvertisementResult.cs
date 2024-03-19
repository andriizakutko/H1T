namespace Common.ServiceResults;

public class TransportAdvertisementResult
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string ModeratorOverviewStatus { get; set; }
    public string Type { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public string BodyType { get; set; }
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
    public string[] Images { get; set; }
}