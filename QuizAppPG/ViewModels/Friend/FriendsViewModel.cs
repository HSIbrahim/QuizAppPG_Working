using QuizAppPG.Views.Friend;
using System.Collections.ObjectModel;

namespace QuizAppPG.ViewModels.Friend
{
    public partial class FriendsViewModel : BaseViewModel
    {
        private readonly IFriendApiService _friendApiService;

        [ObservableProperty]
        private ObservableCollection<FriendDto> friends = new();

        [ObservableProperty]
        private ObservableCollection<FriendDto> pendingRequests = new();

        public FriendsViewModel(
            IFriendApiService friendApiService,
            IDialogService dialogService,
            INavigationService navigationService,
            ISecureStorageService secureStorageService)
            : base(navigationService, dialogService, secureStorageService)
        {
            _friendApiService = friendApiService;
            Title = "Vänner";
        }

        [RelayCommand]
        private async Task LoadFriendsAndRequestsAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var friendsResult = await _friendApiService.GetMyFriendsAsync();
                if (friendsResult.IsSuccess && friendsResult.Data != null)
                {
                    Friends.Clear();
                    foreach (var friend in friendsResult.Data)
                    {
                        Friends.Add(friend);
                    }
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", friendsResult.ErrorMessage ?? "Kunde inte ladda vänner.");
                }
                var pendingResult = await _friendApiService.GetPendingRequestsAsync();
                if (pendingResult.IsSuccess && pendingResult.Data != null)
                {
                    PendingRequests.Clear();
                    foreach (var request in pendingResult.Data)
                    {
                        PendingRequests.Add(request);
                    }
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", pendingResult.ErrorMessage ?? "Kunde inte ladda vänförfrågningar.");
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
        private async Task AcceptFriendRequestAsync(string senderId)
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var result = await _friendApiService.AcceptFriendRequestAsync(senderId);
                if (result.IsSuccess)
                {
                    await _dialogService.ShowAlertAsync("Accepterat", "Vänförfrågan accepterad.");
                    await LoadFriendsAndRequestsAsync();
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kude inte acceptera förfrågan.");
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
        private async Task RejectFriendRequestAsync(string senderId)
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var result = await _friendApiService.RejectFriendRequestAsync(senderId);
                if (result.IsSuccess)
                {
                    await _dialogService.ShowAlertAsync("Nekat", "Vänförfrågan nekades.");
                    await LoadFriendsAndRequestsAsync();
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kunde inte neka förfrågan.");
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
        private async Task RemoveFriendAsync(string friendId)
        {
            if (IsBusy) return;

            var confirm = await _dialogService.ShowConfirmAsync("Ta bort vän", "Är du säker på att du vill ta bort denna vän?", "Ja", "Nej");
            if (!confirm) return;

            IsBusy = true;
            try
            {
                var result = await _friendApiService.RemoveFriendAsync(friendId);
                if (result.IsSuccess)
                {
                    await _dialogService.ShowAlertAsync("Borttagen", "Vän borttagen.");
                    await LoadFriendsAndRequestsAsync();
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kude inte ta bort vän.");
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
        private async Task GoToAddFriendAsync()
        {
            await _navigationService.GoToAsync(nameof(AddFriendPage));
        }
        public override void OnAppearing()
        {
            base.OnAppearing();
            _ = LoadFriendsAndRequestsAsync();
        }
    }
}