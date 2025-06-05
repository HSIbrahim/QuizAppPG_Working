namespace QuizAppPG.ViewModels.User
{
    public partial class ChangePasswordViewModel : BaseViewModel
    {
        private readonly IUserApiService _userApiService;

        [ObservableProperty]
        private string currentPassword = string.Empty;

        [ObservableProperty]
        private string newPassword = string.Empty;

        [ObservableProperty]
        private string confirmNewPassword = string.Empty; 

        public ChangePasswordViewModel(
            IUserApiService userApiService,
            IDialogService dialogService,
            INavigationService navigationService,
            ISecureStorageService secureStorageService)
            : base(navigationService, dialogService, secureStorageService)
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
                    await _navigationService.PopAsync();
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