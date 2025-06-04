using QuizAppPG.ViewModels.Friend;

namespace QuizAppPG.Views.Friend;

public partial class AddFriendPage : ContentPage
{
    public AddFriendPage(AddFriendViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}