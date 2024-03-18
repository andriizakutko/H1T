using Domain.Abstract;

namespace Domain.Transport;

public class TransportTypeMake : BaseEntity
{
    public virtual TransportType TransportType { get; set; }
    public virtual TransportMake TransportMake { get; set; }
}