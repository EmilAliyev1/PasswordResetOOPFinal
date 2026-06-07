using PasswordReset.Interfaces;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace PasswordReset.Services;

public class MultiThreadCracker : IBruteForceCracker
{
    private readonly BruteForceGenerator _bruteForceGenerator;
    // Results from the last cracking run.
    public TimeSpan ElapsedTime { get; private set; }
    public long CheckedCombinationsCount { get; private set; }

    public MultiThreadCracker()
    {
        _bruteForceGenerator = new BruteForceGenerator();
    }

    public async Task<string?> CrackAsync(string targetHash, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(targetHash))
            throw new ArgumentException(nameof(targetHash));

        long totalChecked = 0;
        ElapsedTime = TimeSpan.Zero;
        string? discoveredPassword = null;

        // Keep one CPU core free so the UI can stay responsive.
        int workerCount = Math.Max(1, Environment.ProcessorCount - 1);

        ParallelOptions options = new ParallelOptions
        {
            MaxDegreeOfParallelism = workerCount,
            CancellationToken = ct
        };

        Stopwatch stopwatch = Stopwatch.StartNew();

        try
        {
            await Task.Run(() =>
            {
                // Check many generated passwords at the same time.
                Parallel.ForEach(_bruteForceGenerator.GenerateCombinations(), options, (candidate, state) =>
                {
                    Interlocked.Increment(ref totalChecked);

                    if (PasswordValidator.Validate(candidate, targetHash))
                    {
                        // Only the first thread that found the password should save the password.
                        Interlocked.CompareExchange(ref discoveredPassword, candidate, null);
                        state.Stop();
                    }
                });
            }, ct);
        }
        // Stop when the user cancels the attack.
        catch (OperationCanceledException) { }
        finally
        {
            stopwatch.Stop();
            ElapsedTime = stopwatch.Elapsed;
            CheckedCombinationsCount = totalChecked;
        }

        return discoveredPassword;
    }
}
