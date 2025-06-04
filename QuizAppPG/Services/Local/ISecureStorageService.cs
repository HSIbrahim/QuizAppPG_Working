// Services/Local/ISecureStorageService.cs
namespace QuizAppPG.Services.Local
{
    public interface ISecureStorageService
    {
        Task SaveTokenAsync(string token);
        Task<string?> GetTokenAsync(); // Corrected return type to nullable
        void RemoveToken();

        Task SaveUserIdAsync(string userId);
        Task<string?> GetUserIdAsync(); // Corrected return type to nullable
        void RemoveUserId();

        Task SaveUsernameAsync(string username);
        Task<string?> GetUsernameAsync(); // Corrected return type to nullable
        void ClearAll();
    }
}