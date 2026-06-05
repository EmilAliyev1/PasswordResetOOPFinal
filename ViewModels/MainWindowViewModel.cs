using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using PasswordReset.Services;

namespace PasswordReset.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly PasswordGenerator _passwordGenerator;
    private readonly BruteForceGenerator _bruteForceGenerator;
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
        _bruteForceGenerator = new BruteForceGenerator();

        GenerateRandomPasswordCommand = new RelayCommand(GenerateRandomPassword);
    }

    private void GenerateRandomPassword()
    {
        string plainPassword = _passwordGenerator.GenerateTargetPassword();
        TargetPassword = plainPassword;
        Console.WriteLine(TargetPassword);

        HashedPassword = PasswordHasher.Hash(plainPassword);

        foreach (string candidate in _bruteForceGenerator.GenerateCombinations())
        {
            if (PasswordValidator.Validate(candidate, HashedPassword))
            {
                Console.WriteLine($"BruteForce successful! The password is: {candidate}");
                break;
            }
        }
    }
}
