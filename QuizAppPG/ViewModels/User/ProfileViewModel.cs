using QuizAppPG.Views.User;

namespace QuizAppPG.ViewModels.User
{
    public partial class ProfileViewModel : BaseViewModel
    {
        private readonly IUserApiService _userApiService;

        [ObservableProperty]
        private UserProfileDto? userProfile;

        public ProfileViewModel(
            IUserApiService userApiService,
            ISecureStorageService secureStorageService,
            IDialogService dialogService,
            INavigationService navigationService)
            : base(navigationService, dialogService, secureStorageService)
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
        public override void OnAppearing()
        {
            base.OnAppearing();
            _ = LoadUserProfileAsync();
        }
    }
}