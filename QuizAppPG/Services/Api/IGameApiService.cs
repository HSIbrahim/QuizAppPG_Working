using QuizAppPG.DTOs; // Corrected namespace: CreateGameSessionDto, GameSessionDetailsDto, ServiceResult
using System; // For Guid
using System.Threading.Tasks; // For Task

namespace QuizAppPG.Services.Api
{
    public interface IGameApiService
    {
        Task<ServiceResult<GameSessionDetailsDto>> CreateGameSessionAsync(CreateGameSessionDto createDto);
        Task<ServiceResult<GameSessionDetailsDto>> GetGameSessionDetailsAsync(Guid sessionId);
        Task<ServiceResult> EndGameSessionAsync(Guid sessionId);
    }
}