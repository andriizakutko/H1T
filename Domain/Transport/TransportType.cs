using Domain.Abstract;

namespace Domain.Transport;

public class TransportType : ValueEntity
{
    public virtual ICollection<TransportTypeMake> TransportTypeMakes { get; set; }
    public virtual ICollection<TransportTypeBodyType> TransportTypeBodyTypes { get; set; }
}