namespace Common.Responses;

public class UserInfoResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public bool IsActive { get; set; }
    public ICollection<string> Permissions { get; set; }
}