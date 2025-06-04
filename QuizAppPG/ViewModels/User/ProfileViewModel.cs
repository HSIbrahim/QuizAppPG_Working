using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuizAppPG.DTOs; // Corrected: Use QuizAppPG.DTOs for DTOs
using QuizAppPG.Services.Api;
using QuizAppPG.Services.Local; // For IDialogService, INavigationService, ISecureStorageService
using QuizAppPG.Views.User;
using QuizAppPG.Views.Auth; // Added for LoginPage

namespace QuizAppPG.ViewModels.User
{
    public partial class ProfileViewModel : BaseViewModel
    {
        private readonly IUserApiService _userApiService;
        // _secureStorageService, _dialogService, _navigationService are inherited

        [ObservableProperty]
        private UserProfileDto? userProfile; // Made nullable

        public ProfileViewModel(
            IUserApiService userApiService,
            ISecureStorageService secureStorageService, // <<< ADD THIS PARAMETER
            IDialogService dialogService,
            INavigationService navigationService)
            : base(navigationService, dialogService, secureStorageService) // <<< PASS THIS PARAMETER
        {
            _userApiService = userApiService;
            Title = "Min Profil";
        }

        [RelayCommand]
        private async Task LoadUserProfileAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var result = await _userApiService.GetUserProfileAsync();
                if (result.IsSuccess && result.Data != null)
                {
                    UserProfile = result.Data;
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kude inte ladda användarprofilen.");
                    // Optionally log out or navigate back if profile cannot be loaded
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
        private async Task GoToEditProfileAsync()
        {
            await _navigationService.GoToAsync(nameof(EditProfilePage));
        }

        [RelayCommand]
        private async Task GoToChangePasswordAsync()
        {
            await _navigationService.GoToAsync(nameof(ChangePasswordPage));
        }

        // Removed LogoutAsync command as it's now in BaseViewModel

        public override void OnAppearing() // Override BaseViewModel's method
        {
            base.OnAppearing(); // Call base implementation
            _ = LoadUserProfileAsync(); // Use `_ =` to suppress warning for unawaited Task
        }
    }
}