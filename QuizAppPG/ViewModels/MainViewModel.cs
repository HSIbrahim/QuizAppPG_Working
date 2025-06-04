using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuizAppPG.Services.Local; // For ISecureStorageService

// Removed specific Views usings here, as they are now in BaseViewModel

namespace QuizAppPG.ViewModels
{
    public partial class MainViewModel : BaseViewModel // `partial` and inherits `BaseViewModel`
    {
        // Removed: private readonly ISecureStorageService _secureStorageService; // Now inherited from BaseViewModel
        // Removed: [ObservableProperty] private string welcomeMessage; // Now declared and handled differently

        // Welcome message is specific to MainViewModel's display logic
        [ObservableProperty]
        private string welcomeMessage = string.Empty;


        public MainViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            ISecureStorageService secureStorageService) // Constructor for MainViewModel
            : base(navigationService, dialogService, secureStorageService) // Pass all 3 services to BaseViewModel
        {
            Title = "Välkommen!"; // Title is an inherited property from BaseViewModel
            _ = SetWelcomeMessageAsync(); // Use `_ =` to suppress warning for unawaited Task
        }

        private async Task SetWelcomeMessageAsync()
        {
            // _secureStorageService is inherited
            var username = await _secureStorageService.GetUsernameAsync();
            if (!string.IsNullOrEmpty(username))
            {
                WelcomeMessage = $"Välkommen, {username}!";
            }
            else
            {
                WelcomeMessage = "Välkommen!";
            }
        }

        // Removed: All GoTo... commands are now defined in BaseViewModel and inherited.
        // Removed: LogoutAsync command is now defined in BaseViewModel.

        // Override OnAppearing from BaseViewModel to load specific data
        public override void OnAppearing()
        {
            base.OnAppearing(); // Call base implementation
            _ = SetWelcomeMessageAsync(); // Update welcome message when page appears
        }
    }
}