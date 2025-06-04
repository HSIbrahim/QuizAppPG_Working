using QuizAppPG.ViewModels.Leaderboard;

namespace QuizAppPG.Views.Leaderboard;

public partial class LeaderboardPage : ContentPage
{
    public LeaderboardPage(LeaderboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}