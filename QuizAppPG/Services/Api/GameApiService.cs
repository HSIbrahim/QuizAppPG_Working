using QuizAppPG.DTOs; // Corrected namespace: CreateGameSessionDto, GameSessionDetailsDto, ErrorDto, ServiceResult
using QuizAppPG.Services.Local;
using System.Net.Http; // For HttpMethod, HttpClient
using System; // For Guid

namespace QuizAppPG.Services.Api
{
    public class GameApiService : BaseApiService, IGameApiService
    {
        public GameApiService(HttpClient httpClient, ISecureStorageService secureStorageService)
            : base(httpClient, secureStorageService) { }

        public async Task<ServiceResult<GameSessionDetailsDto>> CreateGameSessionAsync(CreateGameSessionDto createDto)
        {
            // Corrected: Calls SendApiRequestAsync with ONE generic type argument <GameSessionDetailsDto>
            return await SendApiRequestAsync<GameSessionDetailsDto>(HttpMethod.Post, "api/Game/create", createDto);
        }

        public async Task<ServiceResult<GameSessionDetailsDto>> GetGameSessionDetailsAsync(Guid sessionId)
        {
            // Corrected: Calls SendApiRequestAsync with ONE generic type argument <GameSessionDetailsDto>
            return await SendApiRequestAsync<GameSessionDetailsDto>(HttpMethod.Get, $"api/Game/{sessionId}");
        }

        public async Task<ServiceResult> EndGameSessionAsync(Guid sessionId)
        {
            // Corrected: Calls non-generic SendApiRequestAsync
            return await SendApiRequestAsync(HttpMethod.Post, $"api/Game/{sessionId}/end");
        }
    }
}