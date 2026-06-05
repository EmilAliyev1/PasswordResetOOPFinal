using PasswordReset.Interfaces;
using System;

namespace PasswordReset.Services;

public class MultiThreadCracker : IBruteForceCracker
{
    private readonly BruteForceGenerator _bruteForceGenerator;

    // Properties to store the metrics of the cracking operation
    public TimeSpan ElapsedTime { get; private set; }
    public long CheckedCombinationsCount { get; private set; }

    public MultiThreadCracker()
    {
        _bruteForceGenerator = new BruteForceGenerator();
    }

    public string? Crack(string targetHash) {
        throw new NotImplementedException();
    }
}