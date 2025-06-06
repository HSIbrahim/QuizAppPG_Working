﻿using Microsoft.AspNetCore.SignalR.Client;

namespace QuizAppPG.Services.Realtime
{
    public class GameHubClient : IGameHubClient
    {
        private HubConnection? _hubConnection; 
        private readonly ISecureStorageService _secureStorageService;
        private readonly IDialogService _dialogService;

        public event Action<List<GameSessionPlayerDto>>? PlayerJoined;
        public event Action? GameStarted;
        public event Action<QuestionDto, int, bool>? ReceiveQuestion;
        public event Action<AnswerResultDto>? AnswerResult;
        public event Action<string, int>? PlayerScoreUpdated;
        public event Action? GameOver;
        public event Action<string>? ReceiveError;
        public HubConnectionState State => _hubConnection?.State ?? HubConnectionState.Disconnected;


        public GameHubClient(ISecureStorageService secureStorageService, IDialogService dialogService)
        {
            _secureStorageService = secureStorageService;
            _dialogService = dialogService;
        }

        public async Task ConnectAsync()
        {
            if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
            {
                return; 
            }
            if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connecting)
            {
                await Task.Delay(100);
                if (_hubConnection.State == HubConnectionState.Connected) return;
            }


            var token = await _secureStorageService.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                await _dialogService.ShowAlertAsync("Fel", "Ej autentiserad. Vänligen logga in igen.");
                return;
            }

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(Constants.SignalRHubUrl, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<List<GameSessionPlayerDto>>("PlayerJoined", (players) => PlayerJoined?.Invoke(players));
            _hubConnection.On("GameStarted", () => GameStarted?.Invoke());
            _hubConnection.On<QuestionDto, int, bool>("ReceiveQuestion", (question, currentQuestionNumber, isLastQuestion) => ReceiveQuestion?.Invoke(question, currentQuestionNumber, isLastQuestion));
            _hubConnection.On<AnswerResultDto>("AnswerResult", (result) => AnswerResult?.Invoke(result));
            _hubConnection.On<string, int>("PlayerScoreUpdated", (username, score) => PlayerScoreUpdated?.Invoke(username, score));
            _hubConnection.On("GameOver", () => GameOver?.Invoke());
            _hubConnection.On<string>("ReceiveError", (error) => ReceiveError?.Invoke(error));

            _hubConnection.Closed += async (error) =>
            {
                Console.WriteLine($"SignalR Connection Closed: {error?.Message}");
            };
            _hubConnection.Reconnecting += (error) =>
            {
                Console.WriteLine($"SignalR Reconnecting: {error?.Message}");
                return Task.CompletedTask;
            };
            _hubConnection.Reconnected += (connectionId) =>
            {
                Console.WriteLine($"SignalR Reconnected: {connectionId}");
                return Task.CompletedTask;
            };


            try
            {
                await _hubConnection.StartAsync();
                Console.WriteLine("SignalR Connected.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to SignalR: {ex.Message}");
                await _dialogService.ShowAlertAsync("Anslutningsfel", $"Kude inte ansluta till spelserven: {ex.Message}");
            }
        }

        public async Task DisconnectAsync()
        {
            if (_hubConnection != null && _hubConnection.State != HubConnectionState.Disconnected)
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
                _hubConnection = null;
                Console.WriteLine("SignalR Disconnected.");
            }
        }

        public async Task JoinGame(string gameSessionIdString)
        {
            if (_hubConnection?.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("JoinGame", gameSessionIdString);
            }
            else
            {
                await _dialogService.ShowAlertAsync("Fel", "Inte ansluten till spelservrar. Försök igen.");
            }
        }

        public async Task StartGame(string gameSessionIdString)
        {
            if (_hubConnection?.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("StartGame", gameSessionIdString);
            }
            else
            {
                await _dialogService.ShowAlertAsync("Fel", "Inte ansluten till spelservrar. Försök igen.");
            }
        }

        public async Task SubmitAnswer(string gameSessionIdString, SubmitAnswerDto submitAnswerDto)
        {
            if (_hubConnection?.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("SubmitAnswer", gameSessionIdString, submitAnswerDto);
            }
            else
            {
                await _dialogService.ShowAlertAsync("Fel", "Inte ansluten till spelservrar. Försök igen.");
            }
        }

        public async Task RequestNextQuestion(string gameSessionIdString)
        {
            if (_hubConnection?.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("RequestNextQuestion", gameSessionIdString);
            }
            else
            {
                await _dialogService.ShowAlertAsync("Fel", "Inte ansluten till spelservrar. Försök igen.");
            }
        }
    }
}