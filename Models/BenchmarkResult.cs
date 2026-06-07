using System;

namespace PasswordReset.Models;

public record BenchmarkResult(string Password, TimeSpan SingleThreadTime, TimeSpan MultiThreadTime)
{
    // Shows how many times faster the multi-threaded run was.
    public double Speedup => MultiThreadTime.TotalSeconds > 0 ? SingleThreadTime.TotalSeconds / MultiThreadTime.TotalSeconds : 1.0;
}
