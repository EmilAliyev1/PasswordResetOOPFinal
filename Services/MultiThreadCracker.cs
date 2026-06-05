using PasswordReset.Interfaces;
using System;
using System.Diagnostics;
using System.Threading;
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
        if (string.IsNullOrEmpty(targetHash))
            throw new ArgumentException(nameof(targetHash));

        long totalChecked = 0;
        ElapsedTime = TimeSpan.Zero;
        string? discoveredPassword = null;

        int workerCount = Math.Max(1, Environment.ProcessorCount - 1);

        using CancellationTokenSource cts = new CancellationTokenSource();

        ParallelOptions options = new ParallelOptions
        {
            MaxDegreeOfParallelism = workerCount,
            CancellationToken = cts.Token
        };

        Stopwatch stopwatch = Stopwatch.StartNew();

        try
        {
            await Task.Run(() =>
            {
                Parallel.ForEach(_bruteForceGenerator.GenerateCombinations(), options, (candidate, state) =>
                {
                    Interlocked.Increment(ref totalChecked);

                    if (PasswordValidator.Validate(candidate, targetHash))
                    {
                        Interlocked.CompareExchange(ref discoveredPassword, candidate, null);
                        cts.Cancel();
                        state.Stop();
                    }
                });
            });
        }
        // catch the exception from cts.Cancel() and do nothing so that the program does not crash
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