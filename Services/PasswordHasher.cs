using System;
using System.Security.Cryptography;
using System.Text;

namespace PasswordReset.Services;

public static class PasswordHasher
{
    private const string Salt = "STATIC_SALT_2026";

    public static string Hash(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password));

        // Add the same salt every time so matching passwords create matching hashes.
        string combinedInput = password + Salt;
        byte[] inputBytes = Encoding.UTF8.GetBytes(combinedInput);
        byte[] hashBytes = SHA256.HashData(inputBytes);

        StringBuilder builder = new StringBuilder();

        foreach (byte b in hashBytes)
            // Convert each byte to two lowercase hex characters.
            builder.Append(b.ToString("x2"));

        return builder.ToString();
    }
}
