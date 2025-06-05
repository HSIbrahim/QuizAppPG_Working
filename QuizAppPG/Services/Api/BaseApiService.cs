using System.Net.Http.Json; 
using System.Net; 

namespace QuizAppPG.Services.Api
{
    public abstract class BaseApiService
    {
        protected readonly HttpClient _httpClient;
        protected readonly ISecureStorageService _secureStorageService;

        protected BaseApiService(HttpClient httpClient, ISecureStorageService secureStorageService)
        {
            _httpClient = httpClient;
            _secureStorageService = secureStorageService;
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(Constants.BackendUrl);
            }
        }
        protected async Task<string?> GetAuthTokenAsync()
        {
            return await _secureStorageService.GetTokenAsync();
        }
        protected async Task AddAuthHeader(HttpRequestMessage request)
        {
            var token = await GetAuthTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }
        protected async Task<ServiceResult<TResponse>> SendApiRequestAsync<TResponse>(HttpMethod method, string url, object? data = null)
        {
            var request = new HttpRequestMessage(method, url);
            await AddAuthHeader(request);

            if (data != null && (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Patch))
            {
                request.Content = JsonContent.Create(data);
            }

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                TResponse? resultData = await response.Content.ReadFromJsonAsync<TResponse>();
                if (resultData == null)
                {
                    return ServiceResult<TResponse>.Failure("Tom respons från servern vid lyckad operation.");
                }
                return ServiceResult<TResponse>.Success(resultData);
            }
            else
            {
                string errorMessage = $"Fel vid anrop till {url}. Statuskod: {response.StatusCode}";
                string? errorDetails = null;

                try
                {
                    ErrorDto? errorDto = await response.Content.ReadFromJsonAsync<ErrorDto>();
                    if (errorDto != null)
                    {
                        errorMessage = errorDto.Message ?? errorMessage;
                        errorDetails = errorDto.Details;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Kunde inte läsa felrespons som ErrorDto: {ex.Message}");
                    errorMessage = $"Fel: {response.StatusCode}. Kunde inte tolka felmeddelande från servern.";
                }
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    errorMessage = errorMessage.Contains("Ogiltigt användarnamn eller lösenord")
                                 ? "Ogiltigt användarnamn eller lösenord."
                                 : "Du är inte behörig eller din session har upphört. Vänligen logga in igen.";
                }

                return ServiceResult<TResponse>.Failure(errorMessage, errorDetails);
            }
        }
        protected async Task<ServiceResult> SendApiRequestAsync(HttpMethod method, string url, object? data = null)
        {
            var request = new HttpRequestMessage(method, url);
            await AddAuthHeader(request);

            if (data != null && (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Patch))
            {
                request.Content = JsonContent.Create(data);
            }

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return ServiceResult.Success();
            }
            else
            {
                string errorMessage = $"Fel vid anrop till {url}. Statuskod: {response.StatusCode}";
                string? errorDetails = null;

                try
                {
                    ErrorDto? errorDto = await response.Content.ReadFromJsonAsync<ErrorDto>();
                    if (errorDto != null)
                    {
                        errorMessage = errorDto.Message ?? errorMessage;
                        errorDetails = errorDto.Details;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Kunde inte läsa felrespons som ErrorDto: {ex.Message}");
                    errorMessage = $"Fel: {response.StatusCode}. Kunde inte tolka felmeddelande från servern.";
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    errorMessage = "Du är inte behörig eller din session har upphört. Vänligen logga in igen.";
                }

                return ServiceResult.Failure(errorMessage, errorDetails);
            }
        }
    }
}