using System;
using Xamarin.Forms;
using TaskManager.Services;
using TaskManager.Model;

namespace TaskManager
{

    public partial class LoginPage : ContentPage
    {
        private readonly ApiService _apiService = new ApiService();

        public LoginPage()
        {
            InitializeComponent();
            _apiService = new ApiService(); // Initialize API service
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text?.Trim();
            string password = PasswordEntry.Text?.Trim();

            // Validate inputs
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please enter both email and password.", "OK");
                return;
            }

            try
            {
                // Prepare login request payload
                var loginRequest = new LoginRequest
                {
                    Email = email,
                    Password = password
                };

                // Send API request to validate login
                var user = await _apiService.PostAsync<User>("Auth/Login", loginRequest);

                if (user != null && user.UserId > 0)
                {
                    // Display success message
                    await DisplayAlert("Success", "Login successful!", "OK");
                    await _apiService.GenerateNotificationsAsync();

                    // Navigate to HomePage with user details
                    await Navigation.PushModalAsync(new HomePage(user.UserId));
                }
                else
                {
                    await DisplayAlert("Error", "Invalid email or password.", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login Error: {ex.Message}");
                await DisplayAlert("Error", "Login failed. Please try again later.", "OK");
            }
        }

        private async void OnRegisterTapped(object sender, EventArgs e)
        {
            // Navigate to registration page
            await Navigation.PushModalAsync(new RegisterPage());
        }
    }
}
