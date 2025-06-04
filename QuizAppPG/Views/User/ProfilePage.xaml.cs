using QuizAppPG.ViewModels.User;

namespace QuizAppPG.Views.User;

public partial class ProfilePage : ContentPage
{
    private readonly ProfileViewModel _viewModel;
    public ProfilePage(ProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing();
    }
}