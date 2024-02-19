namespace Domain.Interfaces;

public interface IJwtService
{
    Task<string> Generate(User user);
}