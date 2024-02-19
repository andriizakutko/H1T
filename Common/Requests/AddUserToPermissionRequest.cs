namespace Common.Requests;

public class AddUserToPermissionRequest
{
    public string Email { get; set; }
    public string PermissionName { get; set; }
}