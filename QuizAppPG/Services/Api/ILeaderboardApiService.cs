namespace QuizAppPG.Services.Api
{
    public interface ILeaderboardApiService
    {
        Task<ServiceResult<List<LeaderboardEntryDto>>> GetGlobalLeaderboardAsync();
        Task<ServiceResult<List<LeaderboardEntryDto>>> GetCategoryLeaderboardAsync(int categoryId);
    }
}