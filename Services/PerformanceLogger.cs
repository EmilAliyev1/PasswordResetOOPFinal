using System;
using System.IO;
using System.Text;

namespace PasswordReset.Services;

public static class PerformanceLogger
{
    private const string LogFileName = "performance_log.txt";

    public static void LogPerformance(string password, TimeSpan singleThreadTime, TimeSpan multiThreadTime)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException(nameof(password));

        double speedup = multiThreadTime.TotalSeconds > 0 ? singleThreadTime.TotalSeconds / multiThreadTime.TotalSeconds : 1.0;

        StringBuilder logEntry = new StringBuilder();
        logEntry.AppendLine($"Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        logEntry.AppendLine($"Password: {password}");
        logEntry.AppendLine($"Single: {singleThreadTime.TotalSeconds:F2} sec");
        logEntry.AppendLine($"Multi : {multiThreadTime.TotalSeconds:F2} sec");
        logEntry.AppendLine($"Speedup: {speedup:F2}x");
        logEntry.AppendLine(new string('-', 40));

        try
        {
            File.AppendAllText(LogFileName, logEntry.ToString());
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Failed to write data to performance log: {ex.Message}");
        }
    }
}