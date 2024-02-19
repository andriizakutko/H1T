namespace Common.Requests;

public class DeleteUserFromPermissionRequest
{
    public string Email { get; set; }
    public string PermissionName { get; set; }
}