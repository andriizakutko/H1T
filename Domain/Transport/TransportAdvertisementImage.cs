using Domain.Abstract;

namespace Domain.Transport;

public class TransportAdvertisementImage : BaseEntity
{
    public virtual TransportAdvertisement TransportAdvertisement { get; set; }
    public virtual Image Image { get; set; }
}