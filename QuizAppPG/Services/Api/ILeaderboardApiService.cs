using QuizAppPG.DTOs; // Corrected namespace: LeaderboardEntryDto, ServiceResult
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizAppPG.Services.Api
{
    public interface ILeaderboardApiService
    {
        Task<ServiceResult<List<LeaderboardEntryDto>>> GetGlobalLeaderboardAsync();
        Task<ServiceResult<List<LeaderboardEntryDto>>> GetCategoryLeaderboardAsync(int categoryId);
    }
}