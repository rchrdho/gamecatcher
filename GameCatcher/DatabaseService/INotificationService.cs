using System;
using GameCatcher.Models;

namespace GameCatcher.DatabaseService;

public interface INotificationService
{
    Task<List<Notification>> GetUserNotificationsAsync(string userId);

    Task MarkAsReadAsync(int notificationId);

    Task RemoveNotificationAsync(int notificationId);
}
