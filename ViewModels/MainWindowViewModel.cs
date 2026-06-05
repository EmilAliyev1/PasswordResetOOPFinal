using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using PasswordReset.Services;

namespace PasswordReset.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly PasswordGenerator _passwordGenerator;
    private readonly SingleThreadCracker _singleThreadCracker;
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

        GenerateRandomPasswordCommand = new RelayCommand(GenerateRandomPassword);
    }

    private void GenerateRandomPassword()
    {
        string plainPassword = _passwordGenerator.GenerateTargetPassword();
        TargetPassword = plainPassword;
        Console.WriteLine(TargetPassword);

        HashedPassword = PasswordHasher.Hash(plainPassword);

        _singleThreadCracker.Crack(HashedPassword);

        Console.WriteLine($"The amount of time it took: {_singleThreadCracker.ElapsedTime.TotalSeconds:F2}");
        Console.WriteLine($"The amount of combinations: {_singleThreadCracker.CheckedCombinationsCount:N0}");
    }
}
