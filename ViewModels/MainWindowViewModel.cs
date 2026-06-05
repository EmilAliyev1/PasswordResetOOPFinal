using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using PasswordReset.Services;

namespace PasswordReset.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly PasswordGenerator _passwordGenerator;
    private readonly SingleThreadCracker _singleThreadCracker;
    private readonly MultiThreadCracker _multiThreadCracker;
    private string? _targetPassword;
    private string? _hashedPassword;
    public string? TargetPassword
    {
        get => _targetPassword;
        set => SetProperty(ref _targetPassword, value); 
    }
    public string? HashedPassword
    {
        get => _hashedPassword;
        set => SetProperty(ref _hashedPassword, value); 
    }

    public ICommand GenerateRandomPasswordCommand { get; }

    public MainWindowViewModel()
    {
        _passwordGenerator = new PasswordGenerator();
        _singleThreadCracker = new SingleThreadCracker();
        _multiThreadCracker = new MultiThreadCracker();

        GenerateRandomPasswordCommand = new RelayCommand(GenerateRandomPassword);
    }

    private async void GenerateRandomPassword()
    {
        string plainPassword = _passwordGenerator.GenerateTargetPassword();
        TargetPassword = plainPassword;

        HashedPassword = PasswordHasher.Hash(plainPassword);

        await _singleThreadCracker.CrackAsync(HashedPassword);
        await _multiThreadCracker.CrackAsync(HashedPassword);

        PerformanceLogger.LogPerformance(TargetPassword, _singleThreadCracker.ElapsedTime, _multiThreadCracker.ElapsedTime);
    }
}
