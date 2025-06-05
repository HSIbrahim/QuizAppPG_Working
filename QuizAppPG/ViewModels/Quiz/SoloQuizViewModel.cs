namespace QuizAppPG.ViewModels.Quiz
{
    [QueryProperty(nameof(CategoryId), "CategoryId")]
    [QueryProperty(nameof(Difficulty), "Difficulty")]
    public partial class SoloQuizViewModel : BaseViewModel
    {
        private readonly IQuizApiService _quizApiService;

        private List<QuestionDto> _allQuestions = new();
        private int _currentQuestionIndex = 0;

        [ObservableProperty]
        private int categoryId;

        [ObservableProperty]
        private string difficulty = string.Empty;

        [ObservableProperty]
        private QuestionDto? currentQuestion;

        [ObservableProperty]
        private string selectedAnswer = string.Empty;

        [ObservableProperty]
        private int userCurrentScore = 0;

        [ObservableProperty]
        private string feedbackMessage = string.Empty;

        [ObservableProperty]
        private bool isAnswerSubmitted;

        [ObservableProperty]
        private bool showNextButton;

        [ObservableProperty]
        private int questionNumberDisplay;

        [ObservableProperty]
        private int totalQuestionsDisplay;


        public SoloQuizViewModel(
            IQuizApiService quizApiService,
            IDialogService dialogService,
            INavigationService navigationService,
            ISecureStorageService secureStorageService) 
            : base(navigationService, dialogService, secureStorageService)
        {
            _quizApiService = quizApiService;
            Title = "Solo Quiz";
        }

        partial void OnCategoryIdChanged(int value)
        {
            _ = LoadQuestionsAsync();
        }
        private async Task LoadQuestionsAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                if (CategoryId == 0 || string.IsNullOrWhiteSpace(Difficulty))
                {
                    await _dialogService.ShowAlertAsync("Fel", "Quiz-information saknas.");
                    await _navigationService.PopAsync();
                    return;
                }
                if (!Enum.TryParse(Difficulty, true, out DifficultyLevel difficultyEnum))
                {
                    await _dialogService.ShowAlertAsync("Fel", "Ogiltig svårighetsgrad.");
                    await _navigationService.PopAsync();
                    return;
                }

                var result = await _quizApiService.GetQuizQuestionsAsync(CategoryId, Difficulty, 10);
                if (result.IsSuccess && result.Data != null && result.Data.Any())
                {
                    _allQuestions = result.Data;
                    TotalQuestionsDisplay = _allQuestions.Count;
                    _currentQuestionIndex = 0;
                    ShowNextQuestion();
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kude inte ladda frågor för quizet.");
                    await _navigationService.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Fel", $"Ett oväntat fel uppstod: {ex.Message}");
                await _navigationService.PopAsync();
            }
            finally
            {
                IsBusy = false;
                SubmitAnswerCommand.NotifyCanExecuteChanged();
                NextQuestionCommand.NotifyCanExecuteChanged();
            }
        }

        private void ShowNextQuestion()
        {
            if (_currentQuestionIndex < _allQuestions.Count)
            {
                CurrentQuestion = _allQuestions[_currentQuestionIndex];
                QuestionNumberDisplay = _currentQuestionIndex + 1;
                SelectedAnswer = string.Empty;
                FeedbackMessage = string.Empty;
                IsAnswerSubmitted = false;
                ShowNextButton = false;
            }
            else
            {
                _ = _dialogService.ShowAlertAsync("Quiz Slut", $"Quizet är slut! Din totala poäng: {UserCurrentScore}");
                _ = _navigationService.PopAsync();
            }
        }
        [RelayCommand(CanExecute = nameof(CanSubmitAnswer))]
        private async Task SubmitAnswerAsync()
        {
            if (IsBusy || IsAnswerSubmitted) return;

            IsBusy = true;
            try
            {
                if (CurrentQuestion == null || string.IsNullOrWhiteSpace(SelectedAnswer))
                {
                    await _dialogService.ShowAlertAsync("Varning", "Vänligen välj ett svar.");
                    return;
                }

                var submitDto = new SubmitAnswerDto
                {
                    QuestionId = CurrentQuestion.Id,
                    SubmittedAnswer = SelectedAnswer,
                    GameSessionId = Guid.Empty
                };

                var result = await _quizApiService.SubmitSoloAnswerAsync(submitDto);

                if (result.IsSuccess && result.Data != null)
                {
                    IsAnswerSubmitted = true;
                    UserCurrentScore = result.Data.CurrentScore;
                    if (result.Data.IsCorrect)
                    {
                        FeedbackMessage = $"Rätt svar! Du fick {result.Data.PointsAwarded} poäng.";
                    }
                    else
                    {
                        FeedbackMessage = $"Fel svar. Du fick 0 poäng. Det rätta svaret var: {CurrentQuestion.CorrectAnswer}";
                    }
                    ShowNextButton = true;
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kude inte skicka svar.");
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Fel", $"Ett oväntat fel uppstod: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                SubmitAnswerCommand.NotifyCanExecuteChanged();
                NextQuestionCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanSubmitAnswer()
        {
            return !IsBusy && !IsAnswerSubmitted && CurrentQuestion != null;
        }

        [RelayCommand(CanExecute = nameof(CanNextQuestion))]
        private void NextQuestion()
        {
            if (IsBusy) return;

            _currentQuestionIndex++;
            ShowNextQuestion();
            NextQuestionCommand.NotifyCanExecuteChanged();
            SubmitAnswerCommand.NotifyCanExecuteChanged();
        }

        private bool CanNextQuestion()
        {
            return !IsBusy && IsAnswerSubmitted && _currentQuestionIndex < _allQuestions.Count;
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await _navigationService.PopAsync();
        }
    }
}