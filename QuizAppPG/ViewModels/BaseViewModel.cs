using QuizAppPG.Views.Auth;           
using QuizAppPG.Views.Friend;           
using QuizAppPG.Views.Game;            
using QuizAppPG.Views.Leaderboard;      
using QuizAppPG.Views.Quiz;             
using QuizAppPG.Views.User;             

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
        public virtual async Task LogoutAsync() 
        {
            var confirm = await _dialogService.ShowConfirmAsync("Logga ut", "Är du säker på att du vill logga ut?", "Ja", "Nej");
            if (!confirm) return;

            _secureStorageService.ClearAll();
            await _navigationService.GoToAsync($"//{nameof(LoginPage)}", false);
        }
        public virtual void OnAppearing()
        {
        }

        public virtual void OnDisappearing()
        {
        }
    }
}