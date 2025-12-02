using System;
using GameCatcher.Models;

namespace GameCatcher.DatabaseService;

public interface IFriendRequestService
{
    Task SendRequestAsync(string senderId, string receiverId);

    Task AcceptRequestAsync(int requestId);

    Task RejectRequestAsync(int requestId);

    Task<List<FriendRequest>> GetPendingRequestsAsync(string userId);
}
