using PasswordReset.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

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

    public async Task<string?> CrackAsync(string targetHash, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(targetHash))
            throw new ArgumentException(nameof(targetHash));

        long totalChecked = 0;
        ElapsedTime = TimeSpan.Zero;

        Stopwatch stopwatch = Stopwatch.StartNew();

        try
        {
            return await Task.Run(() =>
            {
                foreach (string candidate in _bruteForceGenerator.GenerateCombinations())
                {
                    if (ct.IsCancellationRequested)
                        break;

                    Interlocked.Increment(ref totalChecked);

                    if (PasswordValidator.Validate(candidate, targetHash))
                        return candidate;
                }

                return null;
            });
        }
        finally
        {
            stopwatch.Stop();
            ElapsedTime = stopwatch.Elapsed;
            CheckedCombinationsCount = totalChecked;
        }
    }
}