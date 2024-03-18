using Domain.Abstract;

namespace Domain.Transport;

public class TransportTypeBodyType : BaseEntity
{
    public virtual TransportType TransportType { get; set; }
    public virtual TransportBodyType TransportBodyType { get; set; }
}