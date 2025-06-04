using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuizAppPG.DTOs; // Corrected: Using QuizAppPG.DTOs for DTOs like RegisterDto
using QuizAppPG.Services.Api;
using QuizAppPG.Services.Local; // For IDialogService, INavigationService, ISecureStorageService

namespace QuizAppPG.ViewModels.Auth
{
    public partial class RegisterViewModel : BaseViewModel // Ensure `partial` and inherits `BaseViewModel`
    {
        private readonly IAuthApiService _authApiService;
        // _dialogService, _navigationService, and _secureStorageService are inherited from BaseViewModel

        [ObservableProperty]
        private string username = string.Empty; // Initialized to prevent CS8618

        [ObservableProperty]
        private string email = string.Empty; // Initialized to prevent CS8618

        [ObservableProperty]
        private string password = string.Empty; // Initialized to prevent CS8618

        [ObservableProperty]
        private string confirmPassword = string.Empty; // Initialized to prevent CS8618

        public RegisterViewModel(
            IAuthApiService authApiService,
            IDialogService dialogService,
            INavigationService navigationService,
            ISecureStorageService secureStorageService) // <<< ADD THIS PARAMETER
            : base(navigationService, dialogService, secureStorageService) // <<< PASS THIS PARAMETER
        {
            _authApiService = authApiService;
            Title = "Registrera"; // Title is a property of BaseViewModel
        }

        [RelayCommand]
        private async Task RegisterAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    await _dialogService.ShowAlertAsync("Fel", "Alla fält är obligatoriska.");
                    return;
                }

                if (Password != ConfirmPassword)
                {
                    await _dialogService.ShowAlertAsync("Fel", "Lösenorden matchar inte.");
                    return;
                }

                var registerDto = new RegisterDto { Username = Username, Email = Email, Password = Password };
                var response = await _authApiService.RegisterAsync(registerDto);

                if (response.IsSuccess)
                {
                    await _dialogService.ShowAlertAsync("Framgång", "Registrering lyckades! Du kan nu logga in.");
                    await _navigationService.PopAsync(); // Go back to login page
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", response.Errors?.FirstOrDefault() ?? "Registrering misslyckades.");
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
        private async Task GoBackToLoginAsync()
        {
            await _navigationService.PopAsync();
        }
    }
}