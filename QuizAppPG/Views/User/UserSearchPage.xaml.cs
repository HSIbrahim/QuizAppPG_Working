using QuizAppPG.ViewModels.User;

namespace QuizAppPG.Views.User;

public partial class UserSearchPage : ContentPage
{
    public UserSearchPage(UserSearchViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}