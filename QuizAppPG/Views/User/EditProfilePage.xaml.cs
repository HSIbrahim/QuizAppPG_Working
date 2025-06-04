using QuizAppPG.ViewModels.User;

namespace QuizAppPG.Views.User;

public partial class EditProfilePage : ContentPage
{
    public EditProfilePage(EditProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}