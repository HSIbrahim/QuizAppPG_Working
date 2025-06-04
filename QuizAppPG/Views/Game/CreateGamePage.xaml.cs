using QuizAppPG.ViewModels.Game;

namespace QuizAppPG.Views.Game;

public partial class CreateGamePage : ContentPage
{
    public CreateGamePage(CreateGameViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}