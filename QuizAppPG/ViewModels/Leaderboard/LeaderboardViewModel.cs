using System.Collections.ObjectModel;

namespace QuizAppPG.ViewModels.Leaderboard
{
    public partial class LeaderboardViewModel : BaseViewModel
    {
        private readonly ILeaderboardApiService _leaderboardApiService;
        private readonly IQuizApiService _quizApiService;

        [ObservableProperty]
        private ObservableCollection<LeaderboardEntryDto> leaderboardEntries = new();

        [ObservableProperty]
        private ObservableCollection<QuizCategoryDto> categories = new();

        [ObservableProperty]
        private QuizCategoryDto? selectedCategory;

        [ObservableProperty]
        private bool isGlobalSelected = true;

        public LeaderboardViewModel(
            ILeaderboardApiService leaderboardApiService,
            IQuizApiService quizApiService,
            IDialogService dialogService,
            INavigationService navigationService,
            ISecureStorageService secureStorageService)
            : base(navigationService, dialogService, secureStorageService)
        {
            _leaderboardApiService = leaderboardApiService;
            _quizApiService = quizApiService;
            Title = "Topplistor";

            _ = LoadCategoriesAsync();
            _ = LoadGlobalLeaderboardAsync();
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
                    Categories.Add(new QuizCategoryDto { Id = 0, Name = "Global", Description = "Totala poäng" });
                    foreach (var category in result.Data)
                    {
                        Categories.Add(category);
                    }
                    SelectedCategory = Categories.FirstOrDefault();
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kude inte ladda kategorier för topplistan.");
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
        private async Task LoadLeaderboardAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            LeaderboardEntries.Clear();
            try
            {
                if (SelectedCategory == null) return;

                if (SelectedCategory.Id == 0)
                {
                    var result = await _leaderboardApiService.GetGlobalLeaderboardAsync();
                    if (result.IsSuccess && result.Data != null)
                    {
                        foreach (var entry in result.Data)
                        {
                            LeaderboardEntries.Add(entry);
                        }
                    }
                    else
                    {
                        await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kude inte ladda den globala topplistan.");
                    }
                }
                else
                {
                    var result = await _leaderboardApiService.GetCategoryLeaderboardAsync(SelectedCategory.Id);
                    if (result.IsSuccess && result.Data != null)
                    {
                        foreach (var entry in result.Data)
                        {
                            LeaderboardEntries.Add(entry);
                        }
                    }
                    else
                    {
                        await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kude inte ladda topplistan för kategorin.");
                    }
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
        private async Task LoadGlobalLeaderboardAsync()
        {
            SelectedCategory = Categories.FirstOrDefault(c => c.Id == 0);
            IsGlobalSelected = true;
            await LoadLeaderboardAsync();
        }

        partial void OnSelectedCategoryChanged(QuizCategoryDto? value)
        {
            if (value != null)
            {
                IsGlobalSelected = (value.Id == 0);
                _ = LoadLeaderboardAsync();
            }
        }
    }
}