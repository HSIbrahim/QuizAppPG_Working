namespace QuizAppPG.Services.Api
{
    public class GameApiService : BaseApiService, IGameApiService
    {
        public GameApiService(HttpClient httpClient, ISecureStorageService secureStorageService)
            : base(httpClient, secureStorageService) { }

        public async Task<ServiceResult<GameSessionDetailsDto>> CreateGameSessionAsync(CreateGameSessionDto createDto)
        {
            return await SendApiRequestAsync<GameSessionDetailsDto>(HttpMethod.Post, "api/Game/create", createDto);
        }

        public async Task<ServiceResult<GameSessionDetailsDto>> GetGameSessionDetailsAsync(Guid sessionId)
        {
            return await SendApiRequestAsync<GameSessionDetailsDto>(HttpMethod.Get, $"api/Game/{sessionId}");
        }

        public async Task<ServiceResult> EndGameSessionAsync(Guid sessionId)
        {
            return await SendApiRequestAsync(HttpMethod.Post, $"api/Game/{sessionId}/end");
        }
    }
}