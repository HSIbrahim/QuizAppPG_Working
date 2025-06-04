using System.Runtime.Versioning;
using QuizAppPG.ViewModels.Auth;

namespace QuizAppPG.Views.Auth;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
