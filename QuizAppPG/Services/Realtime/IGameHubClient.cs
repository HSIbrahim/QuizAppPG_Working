using QuizAppPG.DTOs; // Corrected: All DTOs are here (GameSessionPlayerDto, QuestionDto, AnswerResultDto, SubmitAnswerDto)
using System;
using System.Collections.Generic; // For List
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client; // Added for HubConnectionState

namespace QuizAppPG.Services.Realtime
{
    public interface IGameHubClient
    {
        // Events to subscribe to from ViewModels (made nullable to match GameHubClient implementation)
        event Action<List<GameSessionPlayerDto>>? PlayerJoined;
        event Action? GameStarted;
        event Action<QuestionDto, int, bool>? ReceiveQuestion;
        event Action<AnswerResultDto>? AnswerResult;
        event Action<string, int>? PlayerScoreUpdated;
        event Action? GameOver;
        event Action<string>? ReceiveError;

        // NEW: Expose HubConnectionState
        HubConnectionState State { get; }

        // Methods to invoke on the hub
        Task ConnectAsync();
        Task DisconnectAsync();
        Task JoinGame(string gameSessionIdString);
        Task StartGame(string gameSessionIdString);
        Task SubmitAnswer(string gameSessionIdString, SubmitAnswerDto submitAnswerDto);
        Task RequestNextQuestion(string gameSessionIdString);
    }
}