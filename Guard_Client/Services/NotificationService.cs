using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guard_Client.Services
{
    public static class NotificationService
    {
        public static void ShowNotification(string message, string tittle)
        {
            var errorBox = new ErrorFriendlyUserNotification.MainViewModel(tittle, message);
            var window = new ErrorFriendlyUserNotification.MainWindow(errorBox);
            window.Show();
        }
    }
}
