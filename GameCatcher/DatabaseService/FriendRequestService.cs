using GameCatcher.Data;
using GameCatcher.Models;
using Microsoft.EntityFrameworkCore;

namespace GameCatcher.DatabaseService;

public class FriendRequestService : IFriendRequestService
{
    private readonly GameCatcherDbContext _dbContext;
    private readonly IFriendService _friendService;

    public FriendRequestService(
        GameCatcherDbContext gameCatcherDbContext,
        IFriendService friendService
    )
    {
        _dbContext = gameCatcherDbContext;
        _friendService = friendService;
    }

    public async Task SendRequestAsync(string senderId, string receiverId)
    {
        if (senderId == receiverId)
            return;

        var existingRequest = await _dbContext.FriendRequests.AnyAsync(r =>
            r.SenderId == senderId
            && r.ReceiverId == receiverId
            && r.Status == RequestStatus.Pending
        );

        if (existingRequest)
            return;

        var request = new FriendRequest { SenderId = senderId, ReceiverId = receiverId };
        _dbContext.FriendRequests.Add(request);

        var senderName = await _dbContext
            .Users.Where(u => u.Id == senderId)
            .Select(u => u.UserName)
            .FirstOrDefaultAsync();

        var notification = new Notification
        {
            UserId = receiverId,
            Message = $"You have a new friend request from {senderName}.",
        };
        _dbContext.Notifications.Add(notification);

        await _dbContext.SaveChangesAsync();
    }

    public async Task AcceptRequestAsync(int requestId)
    {
        var request = await _dbContext.FriendRequests.FindAsync(requestId);

        if (request is null || request.Status != RequestStatus.Pending)
            return;

        request.Status = RequestStatus.Accepted;

        var receiverName = await _dbContext
            .Users.Where(u => u.Id == request.ReceiverId)
            .Select(u => u.UserName)
            .FirstOrDefaultAsync();

        var notification = new Notification
        {
            UserId = request.SenderId,
            Message = $"{receiverName} accepted your friend request.",
        };
        _dbContext.Notifications.Add(notification);

        await _friendService.AddFriend(request.SenderId!, request.ReceiverId!);

        await _dbContext.SaveChangesAsync();
    }

    public async Task RejectRequestAsync(int requestId)
    {
        var request = await _dbContext.FriendRequests.FindAsync(requestId);

        if (request is not null)
        {
            request.Status = RequestStatus.Rejected;

            _dbContext.FriendRequests.Remove(request);

            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<List<FriendRequest>> GetPendingRequestsAsync(string userId)
    {
        return await _dbContext
            .FriendRequests.Include(r => r.Sender)
            .Where(r => r.ReceiverId == userId && r.Status == RequestStatus.Pending)
            .ToListAsync();
    }
}
