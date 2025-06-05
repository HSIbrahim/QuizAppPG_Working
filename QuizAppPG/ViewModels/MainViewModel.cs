namespace QuizAppPG.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string welcomeMessage = string.Empty;


        public MainViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            ISecureStorageService secureStorageService)
            : base(navigationService, dialogService, secureStorageService)
        {
            Title = "V�lkommen!";
            _ = SetWelcomeMessageAsync();
        }

        private async Task SetWelcomeMessageAsync()
        {
            // _secureStorageService is inherited
            var username = await _secureStorageService.GetUsernameAsync();
            if (!string.IsNullOrEmpty(username))
            {
                WelcomeMessage = $"V�lkommen, {username}!";
            }
            else
            {
                WelcomeMessage = "V�lkommen!";
            }
        }
        public override void OnAppearing()
        {
            base.OnAppearing();
            _ = SetWelcomeMessageAsync();
        }
    }
}