namespace QuizAppPG.ViewModels.User
{
    public partial class EditProfileViewModel : BaseViewModel
    {
        private readonly IUserApiService _userApiService;

        [ObservableProperty]
        private string currentUsername = string.Empty;

        [ObservableProperty]
        private string currentEmail = string.Empty;

        [ObservableProperty]
        private string newUsername = string.Empty;

        [ObservableProperty]
        private string newEmail = string.Empty;

        public EditProfileViewModel(
            IUserApiService userApiService,
            ISecureStorageService secureStorageService,
            IDialogService dialogService,
            INavigationService navigationService)
            : base(navigationService, dialogService, secureStorageService)
        {
            _userApiService = userApiService;
            Title = "Redigera Profil";
            _ = LoadCurrentProfile();
        }

        private async Task LoadCurrentProfile()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var result = await _userApiService.GetUserProfileAsync();
                if (result.IsSuccess && result.Data != null)
                {
                    CurrentUsername = result.Data.Username;
                    CurrentEmail = result.Data.Email;
                    NewUsername = result.Data.Username;
                    NewEmail = result.Data.Email;
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kude inte ladda profil.");
                    await _navigationService.PopAsync();
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
                var updateDto = new UpdateProfileDto
                {
                    NewUsername = (NewUsername != CurrentUsername) ? NewUsername : null,
                    NewEmail = (NewEmail != CurrentEmail) ? NewEmail : null
                };
                if (string.IsNullOrWhiteSpace(updateDto.NewUsername) && string.IsNullOrWhiteSpace(updateDto.NewEmail))
                {
                    await _dialogService.ShowAlertAsync("Inga ändringar", "Inga ändringar upptäcktes att spara.");
                    return;
                }

                var result = await _userApiService.UpdateUserProfileAsync(updateDto);

                if (result.IsSuccess)
                {
                    await _dialogService.ShowAlertAsync("Framgång", "Profil uppdaterad framgångsrikt!");
                    if (!string.IsNullOrWhiteSpace(updateDto.NewUsername))
                    {
                        await _secureStorageService.SaveUsernameAsync(updateDto.NewUsername);
                    }
                    await _navigationService.PopAsync();
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