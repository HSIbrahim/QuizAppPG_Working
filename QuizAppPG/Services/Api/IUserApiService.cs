namespace QuizAppPG.Services.Api
{
    public interface IUserApiService
    {
        Task<ServiceResult<UserProfileDto>> GetUserProfileAsync();
        Task<ServiceResult<UserProfileDto>> GetUserProfileByUsernameAsync(string username);
        Task<ServiceResult<List<UserProfileDto>>> SearchUsersAsync(string query);
        Task<ServiceResult> UpdateUserProfileAsync(UpdateProfileDto updateDto);
        Task<ServiceResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
    }
}