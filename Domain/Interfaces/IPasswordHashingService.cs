namespace Domain.Interfaces;

public interface IPasswordHashingService
{
    string HashPassword(string password, out byte[] salt);
    bool VerifyPassword(string password, string hash, byte[] salt);
}