using System;
using GameCatcher.Data;
using GameCatcher.Models;
using Microsoft.EntityFrameworkCore;

namespace GameCatcher.DatabaseService;

public class NotificationService : INotificationService
{
    private readonly GameCatcherDbContext _dbContext;

    public NotificationService(GameCatcherDbContext gameCatcherDbContext)
    {
        _dbContext = gameCatcherDbContext;
    }

    public async Task<List<Notification>> GetUserNotificationsAsync(string userId)
    {
        return await _dbContext
            .Notifications.Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task MarkAsReadAsync(int notificationId)
    {
        var notification = await _dbContext.Notifications.FindAsync(notificationId);

        if (notification is not null)
        {
            notification.IsRead = true;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task RemoveNotificationAsync(int notificationId)
    {
        var notification = await _dbContext.Notifications.FindAsync(notificationId);

        if (notification is not null)
        {
            _dbContext.Notifications.Remove(notification);
            await _dbContext.SaveChangesAsync();
        }
    }
}
