using Microsoft.AspNetCore.SignalR.Client;

namespace QuizAppPG.Services.Realtime
{
    public interface IGameHubClient
    {
        event Action<List<GameSessionPlayerDto>>? PlayerJoined;
        event Action? GameStarted;
        event Action<QuestionDto, int, bool>? ReceiveQuestion;
        event Action<AnswerResultDto>? AnswerResult;
        event Action<string, int>? PlayerScoreUpdated;
        event Action? GameOver;
        event Action<string>? ReceiveError;
        HubConnectionState State { get; }
        Task ConnectAsync();
        Task DisconnectAsync();
        Task JoinGame(string gameSessionIdString);
        Task StartGame(string gameSessionIdString);
        Task SubmitAnswer(string gameSessionIdString, SubmitAnswerDto submitAnswerDto);
        Task RequestNextQuestion(string gameSessionIdString);
    }
}