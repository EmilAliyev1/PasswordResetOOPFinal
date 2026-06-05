using System;
using System.Threading.Tasks;

namespace PasswordReset.Interfaces;

public interface IBruteForceCracker
{
    TimeSpan ElapsedTime { get; }
    long CheckedCombinationsCount { get; }
    Task<string?> CrackAsync(string targetHash);
}