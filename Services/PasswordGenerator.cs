using System;
using System.Text;

namespace PasswordReset.Services;

public class PasswordGenerator
{
    private const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
    private readonly Random _random;

    public PasswordGenerator()
    {
        _random = new Random();
    }
    
    public string GenerateTargetPassword()
    {
        int passwordLength = _random.Next(4, 6);

        StringBuilder passwordBuilder = new StringBuilder(passwordLength);

        for (int i = 0; i < passwordLength; i++)
        {
            int randomIndex = _random.Next(LowercaseChars.Length);
            passwordBuilder.Append(LowercaseChars[randomIndex]);
        }

        return passwordBuilder.ToString();
    }
}