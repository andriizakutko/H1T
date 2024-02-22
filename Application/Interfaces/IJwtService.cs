using Domain;

namespace Application.Interfaces;

public interface IJwtService
{
    Task<string> Generate(User user);
}