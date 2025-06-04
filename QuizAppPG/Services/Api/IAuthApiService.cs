using QuizAppPG.DTOs; // Corrected namespace: AuthResponseDto, LoginDto, RegisterDto

namespace QuizAppPG.Services.Api
{
    public interface IAuthApiService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    }
}