using QuizAppPG.ViewModels.Game;

namespace QuizAppPG.Views.Game;

public partial class MultiplayerGamePage : ContentPage
{
    private readonly MultiplayerGameViewModel _viewModel;

    public MultiplayerGamePage(MultiplayerGameViewModel viewModel)
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

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.OnDisappearing();
    }
}