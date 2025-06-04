using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuizAppPG.DTOs; // Corrected: Use QuizAppPG.DTOs for DTOs
using QuizAppPG.Services.Api;
using QuizAppPG.Services.Local; // For IDialogService, INavigationService, ISecureStorageService
using QuizAppPG.Utilities; // For DifficultyLevel enum
using System.Collections.ObjectModel;
using System.Linq; // For FirstOrDefault

namespace QuizAppPG.ViewModels.Leaderboard
{
    public partial class LeaderboardViewModel : BaseViewModel
    {
        private readonly ILeaderboardApiService _leaderboardApiService;
        private readonly IQuizApiService _quizApiService; // Needed to get categories for category leaderboard
        // _dialogService, _navigationService, _secureStorageService are inherited

        [ObservableProperty]
        private ObservableCollection<LeaderboardEntryDto> leaderboardEntries = new();

        [ObservableProperty]
        private ObservableCollection<QuizCategoryDto> categories = new();

        [ObservableProperty]
        private QuizCategoryDto? selectedCategory; // Made nullable

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

            _ = LoadCategoriesAsync(); // Load categories on init. Use `_ =` to suppress warning.
            _ = LoadGlobalLeaderboardAsync(); // Load global leaderboard by default. Use `_ =` to suppress warning.
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
                    Categories.Add(new QuizCategoryDto { Id = 0, Name = "Global", Description = "Totala poäng" }); // Add a "Global" option
                    foreach (var category in result.Data)
                    {
                        Categories.Add(category);
                    }
                    SelectedCategory = Categories.FirstOrDefault(); // Select "Global" by default
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
                if (SelectedCategory == null) return; // Should not happen if initialized properly

                if (SelectedCategory.Id == 0) // "Global" category selected
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
                else // Specific category selected
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
            SelectedCategory = Categories.FirstOrDefault(c => c.Id == 0); // Select the "Global" category if available
            IsGlobalSelected = true;
            await LoadLeaderboardAsync();
        }

        partial void OnSelectedCategoryChanged(QuizCategoryDto? value) // ***FIXED: Made `value` nullable to prevent CS8611 warning***
        {
            if (value != null)
            {
                IsGlobalSelected = (value.Id == 0);
                _ = LoadLeaderboardAsync(); // Reload leaderboard when category changes. Use `_ =` to suppress warning.
            }
        }
    }
}