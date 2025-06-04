using QuizAppPG.ViewModels.Game;

namespace QuizAppPG.Views.Game;

public partial class GameLobbyPage : ContentPage
{
    private readonly GameLobbyViewModel _viewModel;

    public GameLobbyPage(GameLobbyViewModel viewModel)
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