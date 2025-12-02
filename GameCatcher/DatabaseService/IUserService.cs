using System;
using GameCatcher.Data;

namespace GameCatcher.DatabaseService;

public interface IUserService
{
    Task<ApplicationUser> GetUserByIdAsync(string userId);
}
