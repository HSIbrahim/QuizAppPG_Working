using QuizAppPG.ViewModels.Quiz;

namespace QuizAppPG.Views.Quiz;

public partial class SoloQuizPage : ContentPage
{
    public SoloQuizPage(SoloQuizViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}