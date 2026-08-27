using Plugin.LocalNotification;
using Plugin.LocalNotification.Core.Models;
using Shelfy.Core;

namespace Shelfy.Services;

public class NotificationService
{
    private const int WarningDaysBefore = 3;

    public async Task RequestPermissionAsync()
    {
        await LocalNotificationCenter.Current.RequestNotificationPermission();
    }

    public async Task ScheduleExpirationNotificationAsync(PantryItem item)
    {
      
        await CancelNotificationAsync(item.Id);

        var warningDate = item.ExpirationDate.Date.AddDays(-WarningDaysBefore);
        var expirationDayDate = item.ExpirationDate.Date;

        if (warningDate > DateTime.Now)
        {
            var warningRequest = new NotificationRequest
            {
                NotificationId = GetWarningNotificationId(item.Id),
                Title = "Son Kullanma Tarihi Yaklaşıyor",
                Description = $"{item.ProductName} ürününün son kullanma tarihine {WarningDaysBefore} gün kaldı.",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = warningDate
                }
            };

            await LocalNotificationCenter.Current.Show(warningRequest);
        }

        if (expirationDayDate > DateTime.Now)
        {
            var expirationRequest = new NotificationRequest
            {
                NotificationId = GetExpirationNotificationId(item.Id),
                Title = "Son Kullanma Tarihi Bugün",
                Description = $"{item.ProductName} ürününün son kullanma tarihi bugün.",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = expirationDayDate
                }
            };

            await LocalNotificationCenter.Current.Show(expirationRequest);
        }
    }

    public Task CancelNotificationAsync(int itemId)
    {
        LocalNotificationCenter.Current.Cancel(GetWarningNotificationId(itemId));
        LocalNotificationCenter.Current.Cancel(GetExpirationNotificationId(itemId));
        return Task.CompletedTask;
    }
    private static int GetWarningNotificationId(int itemId) => itemId * 2;
    private static int GetExpirationNotificationId(int itemId) => itemId * 2 + 1;
}