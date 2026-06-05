using System;
using System.Threading.Tasks;
using System.Threading;

namespace PasswordReset.Interfaces;

public interface IBruteForceCracker
{
    TimeSpan ElapsedTime { get; }
    long CheckedCombinationsCount { get; }
    Task<string?> CrackAsync(string targetHash, CancellationToken ct);
}