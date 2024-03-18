namespace Common.Responses;

public class UsersPermissionsResult
{
    public string Email { get; set; }
    public ICollection<string> Permissions { get; set; }
}