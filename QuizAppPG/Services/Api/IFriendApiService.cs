using QuizAppPG.DTOs; // Corrected namespace: FriendRequestDto, FriendDto, ServiceResult

namespace QuizAppPG.Services.Api
{
    public interface IFriendApiService
    {
        Task<ServiceResult> SendFriendRequestAsync(FriendRequestDto requestDto);
        Task<ServiceResult> AcceptFriendRequestAsync(string senderId);
        Task<ServiceResult> RejectFriendRequestAsync(string senderId);
        Task<ServiceResult> RemoveFriendAsync(string friendId);
        Task<ServiceResult<List<FriendDto>>> GetMyFriendsAsync();
        Task<ServiceResult<List<FriendDto>>> GetPendingRequestsAsync();
    }
}