namespace QuizAppPG.Services.Api
{
    public class UserApiService : BaseApiService, IUserApiService
    {
        public UserApiService(HttpClient httpClient, ISecureStorageService secureStorageService)
            : base(httpClient, secureStorageService) { }

        public async Task<ServiceResult<UserProfileDto>> GetUserProfileAsync()
        {
            return await SendApiRequestAsync<UserProfileDto>(HttpMethod.Get, "api/User/profile");
        }

        public async Task<ServiceResult<UserProfileDto>> GetUserProfileByUsernameAsync(string username)
        {
            return await SendApiRequestAsync<UserProfileDto>(HttpMethod.Get, $"api/User/{username}");
        }

        public async Task<ServiceResult<List<UserProfileDto>>> SearchUsersAsync(string query)
        {
            return await SendApiRequestAsync<List<UserProfileDto>>(HttpMethod.Get, $"api/User/search?query={query}");
        }

        public async Task<ServiceResult> UpdateUserProfileAsync(UpdateProfileDto updateDto)
        {
            return await SendApiRequestAsync(HttpMethod.Patch, "api/User/profile", updateDto);
        }

        public async Task<ServiceResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            return await SendApiRequestAsync(HttpMethod.Post, "api/User/change-password", changePasswordDto);
        }
    }
}