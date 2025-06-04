using QuizAppPG.ViewModels.Friend;

namespace QuizAppPG.Views.Friend;

public partial class FriendsPage : ContentPage
{
    private readonly FriendsViewModel _viewModel;
    public FriendsPage(FriendsViewModel viewModel)
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