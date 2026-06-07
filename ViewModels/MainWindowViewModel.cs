using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using PasswordReset.Services;
using PasswordReset.Interfaces;
using PasswordReset.Models;

namespace PasswordReset.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly PasswordGenerator _passwordGenerator = new PasswordGenerator();

    // Lets the Stop button cancel the current attack.
    private CancellationTokenSource? _cts;

    private string? _targetPassword;
    public string? TargetPassword { get => _targetPassword; set => SetProperty(ref _targetPassword, value); }
    private string? _hashedPassword;
    public string? HashedPassword { get => _hashedPassword;
        set
        {
            // Re-check if the Start button should be enabled.
            if (SetProperty(ref _hashedPassword, value))
                StartAttackAsyncCommand.NotifyCanExecuteChanged();
        }
    }
    private string? _foundPassword;
    public string? FoundPassword { get => _foundPassword; set => SetProperty(ref _foundPassword, value); }
    private string? _elapsedTime;
    public string? ElapsedTime { get => _elapsedTime; set => SetProperty(ref _elapsedTime, value); }
    private string? _performanceResults;
    public string? PerformanceResults { get => _performanceResults; set => SetProperty(ref _performanceResults, value); }
    private bool _isAttacking;
    public bool IsAttacking { get => _isAttacking;
        set
        {
            // Re-check which buttons are allowed during an attack.
            if (SetProperty(ref _isAttacking, value))
            {
                StartAttackAsyncCommand.NotifyCanExecuteChanged();
                StopAttackCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public ICommand GenerateCommand { get; }
    public AsyncRelayCommand StartAttackAsyncCommand { get; }
    public RelayCommand StopAttackCommand { get; }

    public MainWindowViewModel()
    {
        GenerateCommand = new RelayCommand(Generate);
        // Start is enabled only when there is a hash and no attack is running.
        StartAttackAsyncCommand = new AsyncRelayCommand(StartAttackAsync, () => !IsAttacking && !string.IsNullOrEmpty(HashedPassword));
        // Stop is enabled only while an attack is running.
        StopAttackCommand = new RelayCommand(StopAttack, () => IsAttacking);
    }

    private void Generate()
    {
        TargetPassword = _passwordGenerator.GenerateTargetPassword();
        HashedPassword = PasswordHasher.Hash(TargetPassword);
        FoundPassword = "";
        ElapsedTime = "00:00:00";
        PerformanceResults = "Waiting for attack...";
    }

    private async Task StartAttackAsync()
    {
        IsAttacking = true;
        FoundPassword = "Searching...";
        PerformanceResults = "Running performance test...";

        _cts = new CancellationTokenSource();

        try
        {
            IBruteForceCracker singleCracker = new SingleThreadCracker();
            string? singleResult = await singleCracker.CrackAsync(HashedPassword!, _cts.Token);

            IBruteForceCracker multiCracker = new MultiThreadCracker();
            string? multiResult = await multiCracker.CrackAsync(HashedPassword!, _cts.Token);

            // Both runs must finish so the app can compare their times.
            if (multiResult != null && singleResult != null)
            {
                FoundPassword = multiResult;
                ElapsedTime = multiCracker.ElapsedTime.TotalSeconds > singleCracker.ElapsedTime.TotalSeconds
                    ? $"{multiCracker.ElapsedTime.TotalSeconds:F2}s (Multi-threaded)"
                    : $"{singleCracker.ElapsedTime.TotalSeconds:F2}s (Single-threaded)";

                // Store the benchmark data in one object before logging it.
                BenchmarkResult result = new BenchmarkResult(multiResult, singleCracker.ElapsedTime, multiCracker.ElapsedTime);
                PerformanceLogger.LogPerformance(result);

                PerformanceResults = $"Single: {result.SingleThreadTime.TotalSeconds:F2}s | "
                    + $"Multi: {result.MultiThreadTime.TotalSeconds:F2}s | "
                    + $"Speedup: {result.Speedup:F2}x";
            } else
            {
                FoundPassword = "Password not found.";
                PerformanceResults = "Benchmark aborted.";
            }
        }
        catch (OperationCanceledException)
        {
            // This runs when StopAttack cancels the token.
            FoundPassword = "Attack Canceled by User.";
            PerformanceResults = "Benchmark aborted.";
            ElapsedTime = "--";
        }
        catch (Exception ex)
        {
            FoundPassword = $"Error: {ex.Message}";
            ElapsedTime = "--";
        }
        finally
        {
            // Reset the UI state and release the cancellation token.
            IsAttacking = false;
            _cts.Dispose();
            _cts = null;
        }
    }

    private void StopAttack()
    {
        CancellationTokenSource? cts = _cts;
        // Tell all running crackers to stop.
        cts?.Cancel();
    }
}
