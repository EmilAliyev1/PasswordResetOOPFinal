using System;

namespace PasswordReset.Interfaces;

public interface IBruteForceCracker
{
    TimeSpan ElapsedTime { get; }
    long CheckedCombinationsCount { get; }
    string? Crack(string targetHash);
}