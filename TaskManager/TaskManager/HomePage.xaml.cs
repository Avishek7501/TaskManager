using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TaskManager.Services;
using TaskManager.Model;

namespace TaskManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private readonly ApiService _apiService = new ApiService();
        private readonly int _userId;
        private string _userName;

        public HomePage(int userId)
        {
            InitializeComponent();
            _userId = userId;

            // Fetch and display user details
            LoadUserDetails();
        }

        // Fetch user details from API
        private async void LoadUserDetails()
        {
            try
            {
                var user = await _apiService.GetAsync<User>($"User/{_userId}");
                _userName = user.Name;
                WelcomeLabel.Text = $"Welcome, {_userName}";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load user details: {ex.Message}", "OK");
            }
        }

       
       

       

        // Menu Button Clicked
        private async void OnMenuClicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Menu", "Cancel", null, "Account", "Logout");
            if (action == "Account")
            {
                // Navigate to Account Page
                await Navigation.PushAsync(new AccountPage(_userId));
            }
            else if (action == "Logout")
            {
                var confirm = await DisplayAlert("Logout", "Are you sure you want to log out?", "Yes", "No");
                if (confirm)
                {
                    // Log the user out (if necessary, you can also call an API here)
                    await Navigation.PushModalAsync(new LoginPage());
                }
            }
        }

        // Navigate to Notifications Page
        private async void OnNotificationsClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NotificationsPage(_userId));
        }

        // Navigate to Tasks Page
        private async void OnTasksClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TasksPage(_userId));
        }

        // Navigate to Create Task Page
        private async void OnCreateTaskClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CreateTaskPage(_userId));
        }

        private async void OnAccountClicked(object sender, EventArgs e)
        {
            // Navigate to the Account page
            await Navigation.PushModalAsync(new AccountPage(_userId));
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            // Confirm logout
            var confirm = await DisplayAlert("Logout", "Are you sure you want to log out?", "Yes", "No");
            if (confirm)
            {
                // Navigate back to the login page
                await Navigation.PushModalAsync(new LoginPage());
            }
        }
    }
}