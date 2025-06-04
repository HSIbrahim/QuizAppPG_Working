using QuizAppPG.DTOs; // Corrected namespace: UserProfileDto, UpdateProfileDto, ChangePasswordDto, ServiceResult
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizAppPG.Services.Api
{
    public interface IUserApiService
    {
        Task<ServiceResult<UserProfileDto>> GetUserProfileAsync(); // For current user
        Task<ServiceResult<UserProfileDto>> GetUserProfileByUsernameAsync(string username);
        Task<ServiceResult<List<UserProfileDto>>> SearchUsersAsync(string query);
        Task<ServiceResult> UpdateUserProfileAsync(UpdateProfileDto updateDto);
        Task<ServiceResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
    }
}