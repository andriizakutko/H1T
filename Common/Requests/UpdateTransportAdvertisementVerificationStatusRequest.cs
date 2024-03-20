namespace Common.Requests;

public class UpdateTransportAdvertisementVerificationStatusRequest
{
    public Guid TransportAdvertisementId { get; set; }
    public bool IsVerified { get; set; }
}