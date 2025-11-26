using System;
using GameCatcher.Data;
using GameCatcher.Models;

namespace GameCatcher.DatabaseService;

public class UserService : IUserService
{
    private readonly GameCatcherDbContext _dbContext;

    public UserService(GameCatcherDbContext gameCatcherDbContext)
    {
        _dbContext = gameCatcherDbContext;
    }

    public async Task<ApplicationUser> GetUserByIdAsync(string userId)
    {
        return await _dbContext.Users.FindAsync(userId) ?? null!;
    }
}
