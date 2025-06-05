namespace QuizAppPG.ViewModels.Auth
{
    public partial class RegisterViewModel : BaseViewModel
    {
        private readonly IAuthApiService _authApiService;

        [ObservableProperty]
        private string username = string.Empty;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string confirmPassword = string.Empty;

        public RegisterViewModel(
            IAuthApiService authApiService,
            IDialogService dialogService,
            INavigationService navigationService,
            ISecureStorageService secureStorageService)
            : base(navigationService, dialogService, secureStorageService) 
        {
            _authApiService = authApiService;
            Title = "Registrera";
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
                    await _navigationService.PopAsync();
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