using System.Collections.ObjectModel;

namespace QuizAppPG.ViewModels.User
{
    public partial class UserSearchViewModel : BaseViewModel
    {
        private readonly IUserApiService _userApiService;
        private readonly IFriendApiService _friendApiService;

        [ObservableProperty]
        private string searchText = string.Empty;

        [ObservableProperty]
        private ObservableCollection<UserProfileDto> searchResults = new();

        public UserSearchViewModel(
            IUserApiService userApiService,
            IFriendApiService friendApiService,
            IDialogService dialogService,
            INavigationService navigationService,
            ISecureStorageService secureStorageService)
            : base(navigationService, dialogService, secureStorageService)
        {
            _userApiService = userApiService;
            _friendApiService = friendApiService;
            Title = "S�k Anv�ndare";
        }

        [RelayCommand]
        private async Task SearchUsersAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            SearchResults.Clear();
            try
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    await _dialogService.ShowAlertAsync("S�k", "V�nligen ange ett anv�ndarnamn att s�ka efter.");
                    return;
                }

                var result = await _userApiService.SearchUsersAsync(SearchText);
                if (result.IsSuccess && result.Data != null)
                {
                    foreach (var user in result.Data)
                    {
                        SearchResults.Add(user);
                    }
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kude inte s�ka efter anv�ndare.");
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Fel", $"Ett ov�ntat fel uppstod: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SendFriendRequestAsync(string receiverUsername)
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var requestDto = new FriendRequestDto { ReceiverUsername = receiverUsername };
                var result = await _friendApiService.SendFriendRequestAsync(requestDto);

                if (result.IsSuccess)
                {
                    await _dialogService.ShowAlertAsync("V�nf�rfr�gan", $"V�nf�rfr�gan skickades till {receiverUsername}.");
                    var userInList = SearchResults.FirstOrDefault(u => u.Username == receiverUsername);
                    if (userInList != null)
                    {
                        // You could add a flag to UserProfileDto like IsFriendRequestSent
                        // For now, simply reloading search or removing from list might be too aggressive
                    }
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kude inte skicka v�nf�rfr�gan.");
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Fel", $"Ett ov�ntat fel uppstod: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}