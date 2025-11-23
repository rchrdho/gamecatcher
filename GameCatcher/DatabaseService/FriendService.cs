using System;
using GameCatcher.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCatcher.DatabaseService;

public class FriendService : IFriendService
{
    private readonly GameCatcherDbContext _dbContext;

    public FriendService(GameCatcherDbContext gameCatcherDbContext)
    {
        _dbContext = gameCatcherDbContext;
    }

    public Task AddFriend(string userId, string friendId)
    {
        throw new NotImplementedException();
    }

    public Task RemoveFriend(string userId, string friendId)
    {
        throw new NotImplementedException();
    }

    public async Task<ApplicationUser> FindFriendById(string friendId)
    {
        var user =
            await _dbContext.Users.FindAsync(friendId) ?? throw new Exception("User not found");
        return user;
    }

    public async Task<List<ApplicationUser>> GetFriendsByUserId(string userId)
    {
        return await _dbContext.Users.Where(u => u.Friends!.Any(f => f.Id == userId)).ToListAsync();
    }
}
