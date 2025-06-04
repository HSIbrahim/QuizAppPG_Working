using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuizAppPG.DTOs; // Corrected: Use QuizAppPG.DTOs for DTOs
using QuizAppPG.Services.Api;
using QuizAppPG.Services.Local; // For IDialogService, INavigationService, ISecureStorageService

namespace QuizAppPG.ViewModels.User
{
    public partial class EditProfileViewModel : BaseViewModel
    {
        private readonly IUserApiService _userApiService;
        // _secureStorageService, _dialogService, _navigationService are inherited

        [ObservableProperty]
        private string currentUsername = string.Empty; // Initialized

        [ObservableProperty]
        private string currentEmail = string.Empty; // Initialized

        [ObservableProperty]
        private string newUsername = string.Empty; // Initialized

        [ObservableProperty]
        private string newEmail = string.Empty; // Initialized

        public EditProfileViewModel(
            IUserApiService userApiService,
            ISecureStorageService secureStorageService, // <<< ADD THIS PARAMETER
            IDialogService dialogService,
            INavigationService navigationService)
            : base(navigationService, dialogService, secureStorageService) // <<< PASS THIS PARAMETER
        {
            _userApiService = userApiService;
            Title = "Redigera Profil";
            _ = LoadCurrentProfile(); // Use `_ =` to suppress warning for unawaited Task
        }

        private async Task LoadCurrentProfile()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var result = await _userApiService.GetUserProfileAsync(); // Get current user's profile
                if (result.IsSuccess && result.Data != null)
                {
                    CurrentUsername = result.Data.Username;
                    CurrentEmail = result.Data.Email;
                    NewUsername = result.Data.Username; // Pre-fill with current values
                    NewEmail = result.Data.Email;
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kude inte ladda profil.");
                    await _navigationService.PopAsync(); // Go back if profile can't be loaded
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Fel", $"Ett oväntat fel uppstod: {ex.Message}");
                await _navigationService.PopAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SaveProfileAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                // Only send fields that have changed
                var updateDto = new UpdateProfileDto
                {
                    NewUsername = (NewUsername != CurrentUsername) ? NewUsername : null,
                    NewEmail = (NewEmail != CurrentEmail) ? NewEmail : null
                };

                // If nothing changed, just alert and return
                if (string.IsNullOrWhiteSpace(updateDto.NewUsername) && string.IsNullOrWhiteSpace(updateDto.NewEmail))
                {
                    await _dialogService.ShowAlertAsync("Inga ändringar", "Inga ändringar upptäcktes att spara.");
                    return;
                }

                var result = await _userApiService.UpdateUserProfileAsync(updateDto);

                if (result.IsSuccess)
                {
                    await _dialogService.ShowAlertAsync("Framgång", "Profil uppdaterad framgångsrikt!");
                    // Update locally stored username if it changed
                    if (!string.IsNullOrWhiteSpace(updateDto.NewUsername))
                    {
                        await _secureStorageService.SaveUsernameAsync(updateDto.NewUsername); // Use inherited _secureStorageService
                    }
                    await _navigationService.PopAsync(); // Go back after successful update
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kude inte uppdatera profilen.");
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