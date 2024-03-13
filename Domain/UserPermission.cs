namespace Domain;

public class UserPermission : BaseEntity
{
    public virtual User User { get; set; }
    public virtual Permission Permission { get; set; }
}