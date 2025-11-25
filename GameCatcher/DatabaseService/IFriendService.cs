using System;
using GameCatcher.Data;

namespace GameCatcher.DatabaseService;

public interface IFriendService
{
    Task AddFriend(string userId, string friendId);

    Task RemoveFriend(string userId, string friendId);

    Task<List<ApplicationUser>> GetFriendsByUserId(string userId);
}
