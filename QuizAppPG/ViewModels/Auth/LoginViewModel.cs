using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuizAppPG.DTOs; // Corrected: Using QuizAppPG.DTOs for DTOs like LoginDto, AuthResponseDto
using QuizAppPG.Services.Api;
using QuizAppPG.Services.Local;
using QuizAppPG.Views.Auth;
using QuizAppPG.Views; // For MainPage

namespace QuizAppPG.ViewModels.Auth
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IAuthApiService _authApiService;
        // _secureStorageService, _dialogService, and _navigationService are inherited and accessible via protected fields in BaseViewModel

        [ObservableProperty]
        private string username = string.Empty; // Initialized to prevent CS8618

        [ObservableProperty]
        private string password = string.Empty; // Initialized to prevent CS8618

        public LoginViewModel(
            IAuthApiService authApiService,
            ISecureStorageService secureStorageService, // This parameter is needed
            IDialogService dialogService,
            INavigationService navigationService)
            : base(navigationService, dialogService, secureStorageService) // ***FIXED: Pass secureStorageService to base***
        {
            _authApiService = authApiService;
            // _secureStorageService, _dialogService, and _navigationService are now assigned by the base constructor.
            Title = "Logga in";
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    await _dialogService.ShowAlertAsync("Fel", "Användarnamn och lösenord får inte vara tomma.");
                    return;
                }

                var loginDto = new LoginDto { Username = Username, Password = Password };
                var response = await _authApiService.LoginAsync(loginDto);

                if (response.IsSuccess)
                {
                    // Access _secureStorageService directly as it's an inherited protected field
                    await _secureStorageService.SaveTokenAsync(response.Token);
                    await _secureStorageService.SaveUserIdAsync(response.UserId);
                    await _secureStorageService.SaveUsernameAsync(response.Username);

                    await _dialogService.ShowAlertAsync("Framgång", "Inloggning lyckades!");
                    await _navigationService.GoToAsync($"//{nameof(MainPage)}");
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", response.Errors?.FirstOrDefault() ?? "Inloggning misslyckades.");
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
        private async Task GoToRegisterAsync()
        {
            await _navigationService.GoToAsync(nameof(RegisterPage));
        }
    }
}