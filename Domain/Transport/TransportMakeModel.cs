using Domain.Abstract;

namespace Domain.Transport;

public class TransportMakeModel : BaseEntity
{
    public virtual TransportMake TransportMake { get; set; }
    public virtual TransportModel TransportModel { get; set; }
}