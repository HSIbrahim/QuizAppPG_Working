namespace QuizAppPG.Services.Api
{
    public class AuthApiService : BaseApiService, IAuthApiService
    {
        public AuthApiService(HttpClient httpClient, ISecureStorageService secureStorageService)
            : base(httpClient, secureStorageService) { }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var serviceResult = await SendApiRequestAsync<AuthResponseDto>(HttpMethod.Post, "api/Auth/login", loginDto);

            if (serviceResult.IsSuccess && serviceResult.Data != null)
            {
                return serviceResult.Data;
            }
            else
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Errors = serviceResult.Errors ?? new[] { serviceResult.ErrorMessage ?? "Login failed due to an unknown error." }
                };
            }
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var serviceResult = await SendApiRequestAsync<AuthResponseDto>(HttpMethod.Post, "api/Auth/register", registerDto);

            if (serviceResult.IsSuccess && serviceResult.Data != null)
            {
                return serviceResult.Data;
            }
            else
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Errors = serviceResult.Errors ?? new[] { serviceResult.ErrorMessage ?? "Registration failed due to an unknown error." }
                };
            }
        }
    }
}