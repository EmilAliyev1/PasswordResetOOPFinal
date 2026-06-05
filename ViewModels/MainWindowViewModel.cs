using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using PasswordReset.Services;

namespace PasswordReset.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly PasswordGenerator _passwordGenerator;
    private string? _targetPassword;
    public string? TargetPassword
    {
        get => _targetPassword;
        set => SetProperty(ref _targetPassword, value); 
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
    }
}
