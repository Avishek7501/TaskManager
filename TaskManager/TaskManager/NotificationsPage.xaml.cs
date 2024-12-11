using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using TaskManager.Services;
using TaskManager.Model;
using System.Collections.Generic;
using System.Linq;

namespace TaskManager
{
    public partial class NotificationsPage : ContentPage
    {
        private readonly ApiService _apiService = new ApiService();
        private int _userid ;
        public ObservableCollection<Notification> Notifications { get; set; }
        public ICommand MarkAsReadCommand { get; }

        public NotificationsPage(int userid)
        {
            InitializeComponent();
            _userid = userid;
            Notifications = new ObservableCollection<Notification>();
            MarkAsReadCommand = new Command<int>(async (notificationId) => await MarkAsRead(notificationId));
            BindingContext = this;

            // Load unread notifications on page load
            LoadUnreadNotifications();
        }

        private async void LoadUnreadNotifications()
        {
            try
            {
                // Fetch unread notifications from the API
                var notifications = await _apiService.GetAsync<List<Notification>>($"Notification/User/{_userid}/Unread");
                Notifications.Clear();
                foreach (var notification in notifications)
                {
                    Notifications.Add(notification);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load notifications: {ex.Message}", "OK");
            }
        }

        private async Task MarkAsRead(int notificationId)
        {
            try
            {
                // Call the API to mark notification as read
                await _apiService.PutAsync($"Notification/MarkAsRead/{notificationId}");

                // Remove the notification from the list
                var notificationToRemove = Notifications.FirstOrDefault(n => n.NotificationId == notificationId);
                if (notificationToRemove != null)
                {
                    Notifications.Remove(notificationToRemove);
                }

                await DisplayAlert("Success", "Notification marked as read.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to mark notification as read: {ex.Message}", "OK");
            }
        }
    }
}
