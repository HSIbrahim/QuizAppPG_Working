namespace QuizAppPG.Services.Api
{
    public interface IGameApiService
    {
        Task<ServiceResult<GameSessionDetailsDto>> CreateGameSessionAsync(CreateGameSessionDto createDto);
        Task<ServiceResult<GameSessionDetailsDto>> GetGameSessionDetailsAsync(Guid sessionId);
        Task<ServiceResult> EndGameSessionAsync(Guid sessionId);
    }
}