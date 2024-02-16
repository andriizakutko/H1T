using System.Security.Cryptography;
using System.Text;
using Domain.Interfaces;

namespace Application.Services;

public class PasswordHashingService : IPasswordHashingService
{
    private const int KEYSIZE = 64;
    private const int ITERATIONS = 350000;
    private readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
    
    public string HashPassword(string password, out byte[] salt)
    {
        salt = RandomNumberGenerator.GetBytes(KEYSIZE);

        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            ITERATIONS,
            hashAlgorithm,
            KEYSIZE
        );

        return Convert.ToHexString(hash);
    }

    public bool VerifyPassword(string password, string hash, byte[] salt)
    {
        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, ITERATIONS, hashAlgorithm, KEYSIZE);
        return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
    }
}