using QuizAppPG.ViewModels.Quiz;

namespace QuizAppPG.Views.Quiz;

public partial class QuizCategoriesPage : ContentPage
{
    public QuizCategoriesPage(QuizCategoriesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}