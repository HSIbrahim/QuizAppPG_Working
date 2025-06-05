namespace QuizAppPG.Services.Api
{
    public class LeaderboardApiService : BaseApiService, ILeaderboardApiService
    {
        public LeaderboardApiService(HttpClient httpClient, ISecureStorageService secureStorageService)
            : base(httpClient, secureStorageService) { }

        public async Task<ServiceResult<List<LeaderboardEntryDto>>> GetGlobalLeaderboardAsync()
        {
            return await SendApiRequestAsync<List<LeaderboardEntryDto>>(HttpMethod.Get, "api/Leaderboard/global");
        }

        public async Task<ServiceResult<List<LeaderboardEntryDto>>> GetCategoryLeaderboardAsync(int categoryId)
        {
            return await SendApiRequestAsync<List<LeaderboardEntryDto>>(HttpMethod.Get, $"api/Leaderboard/category/{categoryId}");
        }
    }
}