namespace QuizAppPG.Services.Local
{
    public interface ISecureStorageService
    {
        Task SaveTokenAsync(string token);
        Task<string?> GetTokenAsync();
        void RemoveToken();

        Task SaveUserIdAsync(string userId);
        Task<string?> GetUserIdAsync();
        void RemoveUserId();

        Task SaveUsernameAsync(string username);
        Task<string?> GetUsernameAsync(); 
        void ClearAll();
    }
}