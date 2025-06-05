using System.Collections.ObjectModel;

namespace QuizAppPG.ViewModels.Friend
{
    public partial class AddFriendViewModel : BaseViewModel
    {
        private readonly IFriendApiService _friendApiService;
        private readonly IUserApiService _userApiService;

        [ObservableProperty]
        private string searchText = string.Empty;

        [ObservableProperty]
        private ObservableCollection<UserProfileDto> searchResults = new();

        public AddFriendViewModel(
            IFriendApiService friendApiService,
            IUserApiService userApiService,
            IDialogService dialogService,
            INavigationService navigationService,
            ISecureStorageService secureStorageService)
            : base(navigationService, dialogService, secureStorageService)
        {
            _friendApiService = friendApiService;
            _userApiService = userApiService;
            Title = "Lägg till vän";
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
                    await _dialogService.ShowAlertAsync("Sök", "Vänligen ange ett användarnamn att söka efter.");
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
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kunde inte söka efter användare.");
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
                    await _dialogService.ShowAlertAsync("Vänförfrågan", $"Vänförfrågan skickades till {receiverUsername}.");
                    var userInList = SearchResults.FirstOrDefault(u => u.Username == receiverUsername);
                    if (userInList != null)
                    {
                    }
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kunde inte skicka vänförfrågan.");
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