using PasswordReset.Interfaces;
using System;
using System.Threading.Tasks;

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

    public async Task<string?> CrackAsync(string targetHash)
    {
        throw new NotImplementedException();
    }
}