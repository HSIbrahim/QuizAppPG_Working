using QuizAppPG.Views.Leaderboard;
using System.Collections.ObjectModel;
namespace QuizAppPG.ViewModels.Game
{
    [QueryProperty(nameof(GameSessionId), "GameSessionId")]
    public partial class MultiplayerGameViewModel : BaseViewModel
    {
        private readonly IGameHubClient _gameHubClient;

        [ObservableProperty]
        private string gameSessionId = string.Empty;

        [ObservableProperty]
        private QuestionDto? currentQuestion;

        [ObservableProperty]
        private int currentQuestionNumber;

        [ObservableProperty]
        private int totalQuestions;

        [ObservableProperty]
        private string selectedAnswer = string.Empty;

        [ObservableProperty]
        private bool isAnswerSubmitted;

        [ObservableProperty]
        private string feedbackMessage = string.Empty;

        [ObservableProperty]
        private string currentPlayerUsername = string.Empty;

        [ObservableProperty]
        private ObservableCollection<GameSessionPlayerDto> players = new();


        public MultiplayerGameViewModel(
            IGameHubClient gameHubClient,
            IDialogService dialogService,
            INavigationService navigationService,
            ISecureStorageService secureStorageService)
            : base(navigationService, dialogService, secureStorageService)
        {
            _gameHubClient = gameHubClient;
            Title = "Spelar Quiz";
            _gameHubClient.ReceiveQuestion += OnReceiveQuestion;
            _gameHubClient.AnswerResult += OnAnswerResult;
            _gameHubClient.PlayerScoreUpdated += OnPlayerScoreUpdated;
            _gameHubClient.GameOver += OnGameOver;
            _gameHubClient.ReceiveError += OnReceiveError;

            _ = InitializePlayerUsername();
        }

        private async Task InitializePlayerUsername()
        {
            CurrentPlayerUsername = await _secureStorageService.GetUsernameAsync() ?? "Okänd spelare";
        }
        partial void OnGameSessionIdChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _ = RequestNextQuestionAsync();
                TotalQuestions = 10;
            }
        }
        public override void OnAppearing()
        {
            base.OnAppearing();
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            _gameHubClient.ReceiveQuestion -= OnReceiveQuestion;
            _gameHubClient.AnswerResult -= OnAnswerResult;
            _gameHubClient.PlayerScoreUpdated -= OnPlayerScoreUpdated;
            _gameHubClient.GameOver -= OnGameOver;
            _gameHubClient.ReceiveError -= OnReceiveError;
        }
        private void OnReceiveQuestion(QuestionDto question, int questionNumber, bool isLastQuestion)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                CurrentQuestion = question;
                CurrentQuestionNumber = questionNumber;
                SelectedAnswer = string.Empty;
                IsAnswerSubmitted = false;
                FeedbackMessage = string.Empty;
            });
        }

        private void OnAnswerResult(AnswerResultDto result)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                IsAnswerSubmitted = true;
                if (result.IsCorrect)
                {
                    FeedbackMessage = $"Rätt svar! Du fick {result.PointsAwarded} poäng. Din totala poäng: {result.CurrentScore}";
                }
                else
                {
                    FeedbackMessage = $"Fel svar. Du fick 0 poäng. Rätt svar var: {CurrentQuestion?.CorrectAnswer}. Din totala poäng: {result.CurrentScore}";
                }
            });
        }

        private void OnPlayerScoreUpdated(string username, int score)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var player = Players.FirstOrDefault(p => p.Username == username);
                if (player != null)
                {
                    player.Score = score;
                }
            });
        }

        private void OnGameOver()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _dialogService.ShowAlertAsync("Spel Slut", "Spelet är slut! Se resultaten.");
                await _navigationService.GoToAsync($"//{nameof(LeaderboardPage)}");
                await _gameHubClient.DisconnectAsync();
            });
        }

        private void OnReceiveError(string error)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _dialogService.ShowAlertAsync("Fel från server", error);
            });
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
                    GameSessionId = Guid.Parse(GameSessionId)
                };

                await _gameHubClient.SubmitAnswer(GameSessionId, submitDto);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Fel", $"Kude inte skicka svar: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanSubmitAnswer()
        {
            return !IsBusy && !IsAnswerSubmitted && CurrentQuestion != null;
        }


        [RelayCommand(CanExecute = nameof(CanRequestNextQuestion))]
        private async Task RequestNextQuestionAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                await _gameHubClient.RequestNextQuestion(GameSessionId);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Fel", $"Kude inte hämta nästa fråga: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanRequestNextQuestion()
        {
            return !IsBusy;
        }
    }
}