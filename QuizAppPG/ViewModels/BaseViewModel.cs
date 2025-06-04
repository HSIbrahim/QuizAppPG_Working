using CommunityToolkit.Mvvm.ComponentModel; // For ObservableObject and ObservableProperty
using CommunityToolkit.Mvvm.Input;       // For RelayCommand
using QuizAppPG.Services.Local;         // For INavigationService and IDialogService, ISecureStorageService
using QuizAppPG.Views;                  // For MainPage (route)
using QuizAppPG.Views.Auth;             // For LoginPage (route)
using QuizAppPG.Views.Friend;           // For FriendsPage (route)
using QuizAppPG.Views.Game;             // For CreateGamePage (route)
using QuizAppPG.Views.Leaderboard;      // For LeaderboardPage (route)
using QuizAppPG.Views.Quiz;             // For QuizCategoriesPage (route)
using QuizAppPG.Views.User;             // For ProfilePage (route)

namespace QuizAppPG.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        protected readonly INavigationService _navigationService;
        protected readonly IDialogService _dialogService;
        protected readonly ISecureStorageService _secureStorageService;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;

        [ObservableProperty]
        private string title = string.Empty;

        public bool IsNotBusy => !IsBusy;

        public BaseViewModel(INavigationService navigationService, IDialogService dialogService, ISecureStorageService secureStorageService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _secureStorageService = secureStorageService;
        }

        // --- Common Navigation Commands ---
        [RelayCommand]
        protected async Task GoToSoloQuizAsync()
        {
            await _navigationService.GoToAsync(nameof(QuizCategoriesPage));
        }

        [RelayCommand]
        protected async Task GoToCreateGameAsync()
        {
            await _navigationService.GoToAsync(nameof(CreateGamePage));
        }

        [RelayCommand]
        protected async Task GoToFriendsAsync()
        {
            await _navigationService.GoToAsync(nameof(FriendsPage));
        }

        [RelayCommand]
        protected async Task GoToLeaderboardAsync()
        {
            await _navigationService.GoToAsync(nameof(LeaderboardPage));
        }

        [RelayCommand]
        protected async Task GoToProfileAsync()
        {
            await _navigationService.GoToAsync(nameof(ProfilePage));
        }

        [RelayCommand]
        public virtual async Task LogoutAsync() // ***FIXED: Marked as `public virtual`***
        {
            var confirm = await _dialogService.ShowConfirmAsync("Logga ut", "Är du säker på att du vill logga ut?", "Ja", "Nej");
            if (!confirm) return;

            _secureStorageService.ClearAll(); // Clear token and user info
            await _navigationService.GoToAsync($"//{nameof(LoginPage)}", false); // Navigate back to login page
        }

        // --- Common Lifecycle Methods ---
        public virtual void OnAppearing()
        {
            // Base implementation
        }

        public virtual void OnDisappearing()
        {
            // Base implementation
        }
    }
}