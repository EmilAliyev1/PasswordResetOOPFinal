using System;

namespace PasswordReset.Services;

public static class PasswordValidator
{
    public static bool Validate(string candidate, string targetHash)
    {
        if (string.IsNullOrEmpty(candidate))
            throw new ArgumentNullException(nameof(candidate));
            
        if (string.IsNullOrEmpty(targetHash))
            throw new ArgumentNullException(nameof(targetHash));

        string candidateHash = PasswordHasher.Hash(candidate);

        return string.Equals(candidateHash, targetHash, StringComparison.OrdinalIgnoreCase);
    }
}