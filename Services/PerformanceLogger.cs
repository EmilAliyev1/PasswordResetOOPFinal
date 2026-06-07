using System;
using System.IO;
using System.Text;
using PasswordReset.Models;

namespace PasswordReset.Services;

public static class PerformanceLogger
{
    private const string LogFileName = "performance_log.txt";

    public static void LogPerformance(BenchmarkResult result)
    {
        if (string.IsNullOrEmpty(result.Password))
            throw new ArgumentException(nameof(result.Password));

        StringBuilder logEntry = new StringBuilder();
        logEntry.AppendLine($"Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        logEntry.AppendLine($"Password: {result.Password}");
        logEntry.AppendLine($"Single: {result.SingleThreadTime.TotalSeconds:F2} sec");
        logEntry.AppendLine($"Multi : {result.MultiThreadTime.TotalSeconds:F2} sec");
        logEntry.AppendLine($"Speedup: {result.Speedup:F2}x");
        logEntry.AppendLine(new string('-', 40));

        try
        {
            // Add result to the end of the log file.
            File.AppendAllText(LogFileName, logEntry.ToString());
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Failed to write data to performance log: {ex.Message}");
        }
    }
}
