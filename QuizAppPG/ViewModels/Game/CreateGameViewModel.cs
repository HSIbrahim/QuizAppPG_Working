using QuizAppPG.Views.Game;
using System.Collections.ObjectModel;

namespace QuizAppPG.ViewModels.Game
{
    public partial class CreateGameViewModel : BaseViewModel
    {
        private readonly IGameApiService _gameApiService;
        private readonly IQuizApiService _quizApiService;

        [ObservableProperty]
        private ObservableCollection<QuizCategoryDto> categories = new();

        [ObservableProperty]
        private QuizCategoryDto? selectedCategory;

        [ObservableProperty]
        private DifficultyLevel selectedDifficulty = DifficultyLevel.Easy;

        public ObservableCollection<DifficultyLevel> DifficultyLevels { get; } = new(Enum.GetValues(typeof(DifficultyLevel)).Cast<DifficultyLevel>());


        public CreateGameViewModel(
            IGameApiService gameApiService,
            IQuizApiService quizApiService,
            ISecureStorageService secureStorageService,
            IDialogService dialogService,
            INavigationService navigationService)
            : base(navigationService, dialogService, secureStorageService)
        {
            _gameApiService = gameApiService;
            _quizApiService = quizApiService;
            Title = "Skapa Spel";
            _ = LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var result = await _quizApiService.GetQuizCategoriesAsync();
                if (result.IsSuccess && result.Data != null)
                {
                    Categories.Clear();
                    foreach (var category in result.Data)
                    {
                        Categories.Add(category);
                    }
                    SelectedCategory = Categories.FirstOrDefault();
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kunde inte ladda kategorier.");
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Fel", $"Ett oväntat fel uppstod: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task CreateGameSessionAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                if (SelectedCategory == null)
                {
                    await _dialogService.ShowAlertAsync("Val saknas", "Vänligen välj en kategori.");
                    return;
                }

                var createDto = new CreateGameSessionDto
                {
                    QuizCategoryId = SelectedCategory.Id,
                    Difficulty = SelectedDifficulty
                };

                var result = await _gameApiService.CreateGameSessionAsync(createDto);

                if (result.IsSuccess && result.Data != null)
                {
                    await _dialogService.ShowAlertAsync("Framgång", "Spelsession skapad!");
                    var navigationParameters = new Dictionary<string, object>
                    {
                        { "GameSessionId", result.Data.Id.ToString() }
                    };
                    await _navigationService.GoToAsync(nameof(GameLobbyPage), navigationParameters);
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kunde inte skapa spelsession.");
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Fel", $"Ett oväntat fel uppstod: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}