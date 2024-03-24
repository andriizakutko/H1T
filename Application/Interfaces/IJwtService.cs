using Common.Results;
using Domain;

namespace Application.Interfaces;

public interface IJwtService
{
    Task<Result<string>> Generate(User user);
}