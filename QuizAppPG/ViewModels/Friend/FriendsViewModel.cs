using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuizAppPG.DTOs; // Corrected: Use QuizAppPG.DTOs for DTOs
using QuizAppPG.Services.Api;
using QuizAppPG.Services.Local; // For IDialogService, INavigationService, ISecureStorageService
using QuizAppPG.Views.Friend;
using System.Collections.ObjectModel;
using System.Linq; // For LINQ operations

namespace QuizAppPG.ViewModels.Friend
{
    public partial class FriendsViewModel : BaseViewModel
    {
        private readonly IFriendApiService _friendApiService;
        // _dialogService and _navigationService are inherited from BaseViewModel

        [ObservableProperty]
        private ObservableCollection<FriendDto> friends = new();

        [ObservableProperty]
        private ObservableCollection<FriendDto> pendingRequests = new();

        public FriendsViewModel(
            IFriendApiService friendApiService,
            IDialogService dialogService,
            INavigationService navigationService,
            ISecureStorageService secureStorageService) // <<< ADD THIS PARAMETER
            : base(navigationService, dialogService, secureStorageService) // <<< PASS THIS PARAMETER
        {
            _friendApiService = friendApiService;
            Title = "V�nner";
        }

        [RelayCommand]
        private async Task LoadFriendsAndRequestsAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                // Load Friends
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
                    await _dialogService.ShowAlertAsync("Fel", friendsResult.ErrorMessage ?? "Kunde inte ladda v�nner.");
                }

                // Load Pending Requests
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
                    await _dialogService.ShowAlertAsync("Fel", pendingResult.ErrorMessage ?? "Kunde inte ladda v�nf�rfr�gningar.");
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
        private async Task AcceptFriendRequestAsync(string senderId)
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var result = await _friendApiService.AcceptFriendRequestAsync(senderId);
                if (result.IsSuccess)
                {
                    await _dialogService.ShowAlertAsync("Accepterat", "V�nf�rfr�gan accepterad.");
                    await LoadFriendsAndRequestsAsync(); // Refresh lists
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kude inte acceptera f�rfr�gan.");
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
        private async Task RejectFriendRequestAsync(string senderId)
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var result = await _friendApiService.RejectFriendRequestAsync(senderId);
                if (result.IsSuccess)
                {
                    await _dialogService.ShowAlertAsync("Nekat", "V�nf�rfr�gan nekades.");
                    await LoadFriendsAndRequestsAsync(); // Refresh lists
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kunde inte neka f�rfr�gan.");
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
        private async Task RemoveFriendAsync(string friendId)
        {
            if (IsBusy) return;

            var confirm = await _dialogService.ShowConfirmAsync("Ta bort v�n", "�r du s�ker p� att du vill ta bort denna v�n?", "Ja", "Nej");
            if (!confirm) return;

            IsBusy = true;
            try
            {
                var result = await _friendApiService.RemoveFriendAsync(friendId);
                if (result.IsSuccess)
                {
                    await _dialogService.ShowAlertAsync("Borttagen", "V�n borttagen.");
                    await LoadFriendsAndRequestsAsync(); // Refresh lists
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kude inte ta bort v�n.");
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
        private async Task GoToAddFriendAsync()
        {
            await _navigationService.GoToAsync(nameof(AddFriendPage));
        }

        // OnAppearing from BaseViewModel (override is implied if virtual)
        public override void OnAppearing()
        {
            base.OnAppearing(); // Call base implementation
            _ = LoadFriendsAndRequestsAsync(); // Use `_ =` to suppress warning for unawaited Task
        }
    }
}