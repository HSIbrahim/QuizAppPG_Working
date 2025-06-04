using QuizAppPG.DTOs; // Corrected namespace for FriendRequestDto, FriendDto, ErrorDto, ServiceResult
using QuizAppPG.Services.Local;
using System.Net.Http; // For HttpMethod, HttpClient
using System.Collections.Generic; // For List

namespace QuizAppPG.Services.Api
{
    public class FriendApiService : BaseApiService, IFriendApiService
    {
        public FriendApiService(HttpClient httpClient, ISecureStorageService secureStorageService)
            : base(httpClient, secureStorageService) { }

        public async Task<ServiceResult> SendFriendRequestAsync(FriendRequestDto requestDto)
        {
            // Corrected: Calls non-generic SendApiRequestAsync (no generic type arguments)
            return await SendApiRequestAsync(HttpMethod.Post, "api/Friend/send-request", requestDto);
        }

        public async Task<ServiceResult> AcceptFriendRequestAsync(string senderId)
        {
            // Corrected: Calls non-generic SendApiRequestAsync
            return await SendApiRequestAsync(HttpMethod.Post, $"api/Friend/accept-request/{senderId}");
        }

        public async Task<ServiceResult> RejectFriendRequestAsync(string senderId)
        {
            // Corrected: Calls non-generic SendApiRequestAsync
            return await SendApiRequestAsync(HttpMethod.Post, $"api/Friend/reject-request/{senderId}");
        }

        public async Task<ServiceResult> RemoveFriendAsync(string friendId)
        {
            // Corrected: Calls non-generic SendApiRequestAsync
            return await SendApiRequestAsync(HttpMethod.Delete, $"api/Friend/remove-friend/{friendId}");
        }

        public async Task<ServiceResult<List<FriendDto>>> GetMyFriendsAsync()
        {
            // Corrected: Calls SendApiRequestAsync with ONE generic type argument <List<FriendDto>>
            return await SendApiRequestAsync<List<FriendDto>>(HttpMethod.Get, "api/Friend/my-friends");
        }

        public async Task<ServiceResult<List<FriendDto>>> GetPendingRequestsAsync()
        {
            // Corrected: Calls SendApiRequestAsync with ONE generic type argument <List<FriendDto>>
            return await SendApiRequestAsync<List<FriendDto>>(HttpMethod.Get, "api/Friend/pending-requests");
        }
    }
}