namespace Common.Requests;

public class UpdateAdvertisementStatusRequest
{
    public Guid AdvertisementId { get; set; }
    public Guid StatusId { get; set; }
}