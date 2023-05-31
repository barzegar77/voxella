using Api.Services.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Api.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int Iterations = 10000;

        public string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash the password
            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: Iterations,
                numBytesRequested: HashSize
            );

            // Combine the salt and hash into a single string
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Get the bytes of the hashed password
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            // Extract the salt and hash from the combined bytes
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);
            byte[] hash = new byte[HashSize];
            Array.Copy(hashBytes, SaltSize, hash, 0, HashSize);

            // Compute the hash of the provided password
            byte[] computedHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: Iterations,
                numBytesRequested: HashSize
            );

            // Compare the computed hash with the stored hash
            return SlowEquals(hash, computedHash);
        }

        // Compare two byte arrays in a way that doesn't leak timing information
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }
    }
}