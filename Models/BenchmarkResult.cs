using System;

namespace PasswordReset.Models;

public record BenchmarkResult(string Password, TimeSpan SingleThreadTime, TimeSpan MultiThreadTime)
{
    public double Speedup => MultiThreadTime.TotalSeconds > 0 ? SingleThreadTime.TotalSeconds / MultiThreadTime.TotalSeconds : 1.0;
}