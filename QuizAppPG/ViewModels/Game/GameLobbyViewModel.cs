using QuizAppPG.Views.Game;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.SignalR.Client;

namespace QuizAppPG.ViewModels.Game
{
    [QueryProperty(nameof(GameSessionId), "GameSessionId")]
    public partial class GameLobbyViewModel : BaseViewModel
    {
        private readonly IGameApiService _gameApiService;
        private readonly IGameHubClient _gameHubClient;
        [ObservableProperty]
        private string gameSessionId = string.Empty;

        [ObservableProperty]
        private string hostUsername = string.Empty;

        [ObservableProperty]
        private string categoryName = string.Empty;

        [ObservableProperty]
        private string difficulty = string.Empty;

        [ObservableProperty]
        private ObservableCollection<GameSessionPlayerDto> players = new();

        [ObservableProperty]
        private bool isHost;

        public GameLobbyViewModel(
            IGameApiService gameApiService,
            IGameHubClient gameHubClient,
            ISecureStorageService secureStorageService,
            IDialogService dialogService,
            INavigationService navigationService)
            : base(navigationService, dialogService, secureStorageService)
        {
            _gameApiService = gameApiService;
            _gameHubClient = gameHubClient;
            Title = "Spellobby";

            _gameHubClient.PlayerJoined += OnPlayerJoined;
            _gameHubClient.GameStarted += OnGameStarted;
            _gameHubClient.ReceiveError += OnReceiveError;
        }
        partial void OnGameSessionIdChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _ = LoadGameSessionDetailsAndJoinAsync();
            }
        }
        public override void OnAppearing()
        {
            base.OnAppearing();
            _ = ConnectAndJoinGameHubAsync();
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            _gameHubClient.PlayerJoined -= OnPlayerJoined;
            _gameHubClient.GameStarted -= OnGameStarted;
            _gameHubClient.ReceiveError -= OnReceiveError;
        }
        private async Task ConnectAndJoinGameHubAsync()
        {
            await _gameHubClient.ConnectAsync();
            if (_gameHubClient.State == HubConnectionState.Connected)
            {
                await _gameHubClient.JoinGame(GameSessionId);
            }
            else
            {
                await _dialogService.ShowAlertAsync("Anslutningsfel", "Kunde inte ansluta till spelets hub.");
            }
        }

        private async Task LoadGameSessionDetailsAndJoinAsync()
        {
            if (IsBusy) return;
            if (string.IsNullOrEmpty(GameSessionId)) return;

            IsBusy = true;
            try
            {
                var result = await _gameApiService.GetGameSessionDetailsAsync(Guid.Parse(GameSessionId));
                if (result.IsSuccess && result.Data != null)
                {
                    HostUsername = result.Data.HostUsername;
                    CategoryName = result.Data.CategoryName;
                    Difficulty = result.Data.Difficulty.ToString();
                    Players.Clear();
                    foreach (var player in result.Data.Players)
                    {
                        Players.Add(player);
                    }

                    var currentUsernameFromStorage = await _secureStorageService.GetUsernameAsync();
                    IsHost = result.Data.HostUsername == currentUsernameFromStorage;
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Fel", result.ErrorMessage ?? "Kude inte ladda spelsessionens detaljer.");
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
        private void OnPlayerJoined(List<GameSessionPlayerDto> updatedPlayers)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Players.Clear();
                foreach (var player in updatedPlayers)
                {
                    Players.Add(player);
                }
                _dialogService.ShowAlertAsync("Spelare anslöt", $"{updatedPlayers.LastOrDefault()?.Username} anslöt till lobbyn.");
            });
        }

        private async void OnGameStarted()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _dialogService.ShowAlertAsync("Spel Startat!", "Spelet har börjat!");
                var navigationParameters = new Dictionary<string, object>
                {
                    { "GameSessionId", GameSessionId }
                };
                await _navigationService.GoToAsync(nameof(MultiplayerGamePage), navigationParameters);
            });
        }

        private void OnReceiveError(string error)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _dialogService.ShowAlertAsync("Fel från server", error);
            });
        }
        [RelayCommand]
        private async Task StartGameAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                await _gameHubClient.StartGame(GameSessionId);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Fel", $"Kude inte starta spelet: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task LeaveLobbyAsync()
        {
            var confirm = await _dialogService.ShowConfirmAsync("Lämna lobbyn", "Är du säker på att du vill lämna lobbyn?", "Ja", "Nej");
            if (!confirm) return;

            await _gameHubClient.DisconnectAsync();
            await _navigationService.PopAsync();
        }
    }
}