namespace QuizAppPG.Services.Api
{
    public class FriendApiService : BaseApiService, IFriendApiService
    {
        public FriendApiService(HttpClient httpClient, ISecureStorageService secureStorageService)
            : base(httpClient, secureStorageService) { }

        public async Task<ServiceResult> SendFriendRequestAsync(FriendRequestDto requestDto)
        {
            return await SendApiRequestAsync(HttpMethod.Post, "api/Friend/send-request", requestDto);
        }

        public async Task<ServiceResult> AcceptFriendRequestAsync(string senderId)
        {
            return await SendApiRequestAsync(HttpMethod.Post, $"api/Friend/accept-request/{senderId}");
        }

        public async Task<ServiceResult> RejectFriendRequestAsync(string senderId)
        {
            return await SendApiRequestAsync(HttpMethod.Post, $"api/Friend/reject-request/{senderId}");
        }

        public async Task<ServiceResult> RemoveFriendAsync(string friendId)
        {
            return await SendApiRequestAsync(HttpMethod.Delete, $"api/Friend/remove-friend/{friendId}");
        }

        public async Task<ServiceResult<List<FriendDto>>> GetMyFriendsAsync()
        {
            return await SendApiRequestAsync<List<FriendDto>>(HttpMethod.Get, "api/Friend/my-friends");
        }

        public async Task<ServiceResult<List<FriendDto>>> GetPendingRequestsAsync()
        {
            return await SendApiRequestAsync<List<FriendDto>>(HttpMethod.Get, "api/Friend/pending-requests");
        }
    }
}