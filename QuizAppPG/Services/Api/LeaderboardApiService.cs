using QuizAppPG.DTOs; // Corrected namespace: LeaderboardEntryDto, ErrorDto, ServiceResult
using QuizAppPG.Services.Local;
using System.Net.Http; // For HttpMethod, HttpClient
using System.Collections.Generic; // For List

namespace QuizAppPG.Services.Api
{
    public class LeaderboardApiService : BaseApiService, ILeaderboardApiService
    {
        public LeaderboardApiService(HttpClient httpClient, ISecureStorageService secureStorageService)
            : base(httpClient, secureStorageService) { }

        public async Task<ServiceResult<List<LeaderboardEntryDto>>> GetGlobalLeaderboardAsync()
        {
            // Corrected: Calls SendApiRequestAsync with ONE generic type argument <List<LeaderboardEntryDto>>
            return await SendApiRequestAsync<List<LeaderboardEntryDto>>(HttpMethod.Get, "api/Leaderboard/global");
        }

        public async Task<ServiceResult<List<LeaderboardEntryDto>>> GetCategoryLeaderboardAsync(int categoryId)
        {
            // Corrected: Calls SendApiRequestAsync with ONE generic type argument <List<LeaderboardEntryDto>>
            return await SendApiRequestAsync<List<LeaderboardEntryDto>>(HttpMethod.Get, $"api/Leaderboard/category/{categoryId}");
        }
    }
}