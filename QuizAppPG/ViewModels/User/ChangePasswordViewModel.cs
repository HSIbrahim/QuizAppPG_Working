using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuizAppPG.DTOs; // Corrected: Use QuizAppPG.DTOs for DTOs
using QuizAppPG.Services.Api;
using QuizAppPG.Services.Local; // For IDialogService, INavigationService, ISecureStorageService

namespace QuizAppPG.ViewModels.User
{
    public partial class ChangePasswordViewModel : BaseViewModel
    {
        private readonly IUserApiService _userApiService;
        // _dialogService and _navigationService are inherited

        [ObservableProperty]
        private string currentPassword = string.Empty; // Initialized

        [ObservableProperty]
        private string newPassword = string.Empty; // Initialized

        [ObservableProperty]
        private string confirmNewPassword = string.Empty; // Initialized

        public ChangePasswordViewModel(
            IUserApiService userApiService,
            IDialogService dialogService,
            INavigationService navigationService,
            ISecureStorageService secureStorageService) // <<< ADD THIS PARAMETER
            : base(navigationService, dialogService, secureStorageService) // <<< PASS THIS PARAMETER
        {
            _userApiService = userApiService;
            Title = "Ändra Lösenord";
        }

        [RelayCommand]
        private async Task ChangePasswordAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                if (string.IsNullOrWhiteSpace(CurrentPassword) || string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(ConfirmNewPassword))
                {
                    await _dialogService.ShowAlertAsync("Fel", "Alla lösenordsfält är obligatoriska.");
                    return;
                }

                if (NewPassword != ConfirmNewPassword)
                {
                    await _dialogService.ShowAlertAsync("Fel", "Nya lösenord matchar inte.");
                    return;
                }

                if (NewPassword == CurrentPassword)
                {
                    await _dialogService.ShowAlertAsync("Varning", "Nya lösenordet kan inte vara detsamma som det nuvarande.");
                    return;
                }

                var changeDto = new ChangePasswordDto
                {
                    CurrentPassword = CurrentPassword,
                    NewPassword = NewPassword,
                    ConfirmNewPassword = ConfirmNewPassword
                };

                var result = await _userApiService.ChangePasswordAsync(changeDto);

                if (result.IsSuccess)
                {
                    await _dialogService.ShowAlertAsync("Framgång", "Lösenord ändrat framgångsrikt!");
                    await _navigationService.PopAsync(); // Go back after successful change
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kude inte ändra lösenord.");
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