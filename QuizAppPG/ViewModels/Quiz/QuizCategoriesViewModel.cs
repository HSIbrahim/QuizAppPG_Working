using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuizAppPG.DTOs; // Corrected: Use QuizAppPG.DTOs for DTOs
using QuizAppPG.Services.Api;
using QuizAppPG.Services.Local; // For IDialogService, INavigationService, ISecureStorageService
using QuizAppPG.Utilities; // For DifficultyLevel enum
using QuizAppPG.Views.Quiz;
using System.Collections.ObjectModel;
using System.Linq; // For Cast<T>

namespace QuizAppPG.ViewModels.Quiz
{
    public partial class QuizCategoriesViewModel : BaseViewModel
    {
        private readonly IQuizApiService _quizApiService;
        // _dialogService and _navigationService are inherited

        [ObservableProperty]
        private ObservableCollection<QuizCategoryDto> categories = new();

        [ObservableProperty]
        private QuizCategoryDto? selectedCategory; // Made nullable

        [ObservableProperty]
        private DifficultyLevel selectedDifficulty = DifficultyLevel.Easy; // Default

        public ObservableCollection<DifficultyLevel> DifficultyLevels { get; } = new(Enum.GetValues(typeof(DifficultyLevel)).Cast<DifficultyLevel>());


        public QuizCategoriesViewModel(
            IQuizApiService quizApiService,
            IDialogService dialogService,
            INavigationService navigationService,
            ISecureStorageService secureStorageService) // <<< ADD THIS PARAMETER
            : base(navigationService, dialogService, secureStorageService) // <<< PASS THIS PARAMETER
        {
            _quizApiService = quizApiService;
            Title = "Välj Kategori";
            _ = LoadCategoriesAsync(); // Load categories on init. Use `_ =` to suppress warning.
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
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kude inte ladda kategorier.");
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
        private async Task StartSoloQuizAsync()
        {
            if (IsBusy) return;

            if (SelectedCategory == null)
            {
                await _dialogService.ShowAlertAsync("Val saknas", "Vänligen välj en kategori.");
                return;
            }

            IsBusy = true;
            try
            {
                var navigationParameters = new Dictionary<string, object>
                {
                    { "CategoryId", SelectedCategory.Id },
                    { "Difficulty", SelectedDifficulty.ToString() }
                };
                await _navigationService.GoToAsync(nameof(SoloQuizPage), navigationParameters);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Fel", $"Kude inte starta quiz: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}