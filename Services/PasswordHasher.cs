using System;
using System.Security.Cryptography;
using System.Text;

namespace PasswordReset.Services;

public static class PasswordHasher
{
    private const string Salt = "STATIC_SALT_2026";

    public static string Hash(string password)
    {
        if (password == null)
            throw new ArgumentNullException(nameof(password));

        string combinedInput = password + Salt;
        byte[] inputBytes = Encoding.UTF8.GetBytes(combinedInput);
        byte[] hashBytes = SHA256.HashData(inputBytes);

        StringBuilder builder = new StringBuilder();

        foreach (byte b in hashBytes)
            builder.Append(b.ToString("x2"));

        return builder.ToString();
    }
}