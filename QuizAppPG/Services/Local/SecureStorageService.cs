namespace QuizAppPG.Services.Local
{
    public class SecureStorageService : ISecureStorageService
    {
        private const string AuthTokenKey = "auth_token";
        private const string UserIdKey = "user_id";
        private const string UsernameKey = "username";

        public async Task SaveTokenAsync(string token) => await SecureStorage.SetAsync(AuthTokenKey, token);
        public async Task<string?> GetTokenAsync() => await SecureStorage.GetAsync(AuthTokenKey);
        public void RemoveToken() => SecureStorage.Remove(AuthTokenKey);

        public async Task SaveUserIdAsync(string userId) => await SecureStorage.SetAsync(UserIdKey, userId);
        public async Task<string?> GetUserIdAsync() => await SecureStorage.GetAsync(UserIdKey);
        public void RemoveUserId() => SecureStorage.Remove(UserIdKey);

        public async Task SaveUsernameAsync(string username) => await SecureStorage.SetAsync(UsernameKey, username);
        public async Task<string?> GetUsernameAsync() => await SecureStorage.GetAsync(UsernameKey);
        public void ClearAll()
        {
            SecureStorage.Remove(AuthTokenKey);
            SecureStorage.Remove(UserIdKey);
            SecureStorage.Remove(UsernameKey);
        }
    }
}