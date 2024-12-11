using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TaskManager.Model;
using TaskManager.Services;

namespace TaskManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountPage : ContentPage
    {
        private readonly ApiService _apiService = new ApiService();
        private readonly int _userId;

        public AccountPage(int userId)
        {
            InitializeComponent();
            _userId = userId;

            // Load user details
            LoadUserDetails();
        }

        private async void LoadUserDetails()
        {
            try
            {
                // Fetch user details
                var user = await _apiService.GetAsync<User>($"User/{_userId}");
                if (user != null)
                {
                    NameLabel.Text = user.Name;
                    EmailLabel.Text = user.Email;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load account details: {ex.Message}", "OK");
            }
        }

        private async void OnDeleteAccountClicked(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Delete Account", "Are you sure you want to delete your account? This action cannot be undone.", "Yes", "No");
            if (!confirm) return;

            try
            {
                // Call API to delete user
                await _apiService.DeleteAsync($"User/{_userId}");

                await DisplayAlert("Account Deleted", "Your account has been deleted successfully.", "OK");

                // Log the user out and navigate to the login page
                await Navigation.PushModalAsync(new LoginPage());

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to delete account: {ex.Message}", "OK");
            }
        }
    }
}