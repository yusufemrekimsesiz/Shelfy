using Plugin.LocalNotification;
using Plugin.LocalNotification.Core.Models;
using Shelfy.Core;

namespace Shelfy.Services;

public class NotificationService
{
    public async Task RequestPermissionAsync()
    {
        await LocalNotificationCenter.Current.RequestNotificationPermission();
    }

    public async Task ScheduleExpirationNotificationAsync(PantryItem item)
    {
        var notifyDate = item.ExpirationDate.AddDays(-2);

        if (notifyDate <= DateTime.Now)
            return;

        var request = new NotificationRequest
        {
            NotificationId = item.Id,
            Title = "Son Kullanma Tarihi Yaklaşıyor",
            Description = $"{item.ProductName} ürününün son kullanma tarihine 2 gün kaldı.",
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = notifyDate
            }
        };

        await LocalNotificationCenter.Current.Show(request);
    }

    public Task CancelNotificationAsync(int itemId)
    {
        LocalNotificationCenter.Current.Cancel(itemId);
        return Task.CompletedTask;
    }
}