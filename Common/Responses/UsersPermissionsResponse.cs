namespace Common.Responses;

public class UsersPermissionsResponse
{
    public string Email { get; set; }
    public ICollection<string> Permissions { get; set; }
}