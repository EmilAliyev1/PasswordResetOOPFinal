using PasswordReset.Interfaces;
using System;
using System.Diagnostics;

namespace PasswordReset.Services;

public class SingleThreadCracker : IBruteForceCracker
{
    private readonly BruteForceGenerator _bruteForceGenerator;

    // Properties to store the metrics of the cracking operation
    public TimeSpan ElapsedTime { get; private set; }
    public long CheckedCombinationsCount { get; private set; }

    public SingleThreadCracker()
    {
        _bruteForceGenerator = new BruteForceGenerator();
    }

    public string? Crack(string targetHash)
    {
        if (targetHash == null)
            throw new ArgumentException(nameof(targetHash));

        CheckedCombinationsCount = 0;
        ElapsedTime = TimeSpan.Zero;

        Stopwatch stopwatch = Stopwatch.StartNew();

        try
        {
            foreach (string candidate in _bruteForceGenerator.GenerateCombinations())
            {
                CheckedCombinationsCount++;

                if (PasswordValidator.Validate(candidate, targetHash))
                    return candidate;
            }

            return null;
        }
        finally
        {
            stopwatch.Stop();
            ElapsedTime = stopwatch.Elapsed;
        }
    }
}