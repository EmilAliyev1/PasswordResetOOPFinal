using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using PasswordReset.Services;

namespace PasswordReset.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly PasswordGenerator _passwordGenerator;
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

        GenerateRandomPasswordCommand = new RelayCommand(GenerateRandomPassword);
    }

    private void GenerateRandomPassword()
    {
        string plainPassword = _passwordGenerator.GenerateTargetPassword();
        TargetPassword = plainPassword;

        HashedPassword = PasswordHasher.Hash(plainPassword);

        string correctTargetHash = "185ee5f0dd3e66ea4ccc5232275e1eb7e9d313718e52d2f29ce74ed849149e54";

        bool flag = PasswordValidator.Validate("test", correctTargetHash);

        Console.WriteLine(flag);
    }
}
