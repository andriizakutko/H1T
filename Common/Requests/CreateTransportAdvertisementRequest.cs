namespace Common.Requests;

public class CreateTransportAdvertisementRequest
{
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public Guid TypeId { get; set; }
    public Guid MakeId { get; set; }
    public Guid ModelId { get; set; }
    public Guid BodyTypeId { get; set; }
    public string CreatorEmail { get; set; }
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
    public string[] ImageUrls { get; set; }
}