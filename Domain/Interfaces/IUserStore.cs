namespace Domain.Interfaces;

public interface IUserStore
{
    Task<bool> Create(User user);
    Task<User> GetByEmail(string email);
    Task<bool> IsEmailExist(string email);
}