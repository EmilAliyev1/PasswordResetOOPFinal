using System;

namespace PasswordReset.Services;

public static class PasswordValidator
{
    public static bool Validate(string candidate, string targetHash)
    {
        if (candidate == null)
            throw new ArgumentNullException(nameof(candidate));
            
        if (targetHash == null)
            throw new ArgumentNullException(nameof(targetHash));

        string candidateHash = PasswordHasher.Hash(candidate);

        return string.Equals(candidateHash, targetHash, StringComparison.OrdinalIgnoreCase);
    }
}