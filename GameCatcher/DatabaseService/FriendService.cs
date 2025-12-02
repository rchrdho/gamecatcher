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

    public async Task AddFriend(string userId, string friendId)
    {
        await _dbContext.Database.ExecuteSqlRawAsync(
            "INSERT INTO UserFriends (FriendsId, FriendOfId) VALUES ({0}, {1})",
            friendId,
            userId
        );
    }

    public async Task RemoveFriend(string userId, string friendId)
    {
        await _dbContext.Database.ExecuteSqlRawAsync(
            "DELETE FROM UserFriends WHERE FriendsId = {0} AND FriendOfId = {1}",
            friendId,
            userId
        );
    }

    public async Task<List<ApplicationUser>> GetFriendsByUserId(string userId)
    {
        var users = await _dbContext
            .Users.Where(u => u.Id == userId)
            .Include(u => u.Friends)
            .ToListAsync();

        return users.FirstOrDefault()!.Friends!.ToList();
    }

    public async Task<List<ApplicationUser>> GetFriendsOfUserByUserId(string userId)
    {
        var users = await _dbContext
            .Users.Where(u => u.Id == userId)
            .Include(u => u.FriendOf)
            .ToListAsync();

        return users.FirstOrDefault()!.FriendOf!.ToList();
    }
}
