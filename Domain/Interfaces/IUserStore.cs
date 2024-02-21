namespace Domain.Interfaces;

public interface IUserStore
{
    Task<IEnumerable<User>> GetAll();
    Task<User> Create(User user);
    Task<User> GetByEmail(string email);
    Task<bool> IsEmailExist(string email);
}