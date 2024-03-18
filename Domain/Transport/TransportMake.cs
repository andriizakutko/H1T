using Domain.Abstract;

namespace Domain.Transport;

public class TransportMake : ValueEntity
{
    public virtual ICollection<TransportMakeModel> TransportMakeModels { get; set; }
}