using System;
using Xamarin.Forms;
using TaskManager.Model;
using TaskManager.Services;

namespace TaskManager
{
    public partial class RegisterPage : ContentPage
    {
        private readonly ApiService _apiService = new ApiService();

        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            // Retrieve user inputs
            string name = NameEntry.Text;
            string email = EmailEntry.Text;
            string password = PasswordEntry.Text;
            string confirmPassword = ConfirmPasswordEntry.Text;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "All fields are required.", "OK");
                return;
            }

            if (password != confirmPassword)
            {
                await DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            // Create a User object
            var newUser = new User
            {
                Name = name,
                Email = email,
                Password = password
            };

            try
            {
                // Call the API to register the user
                var response = await _apiService.PostAsync<User>("User/Register", newUser);

                // Show success message
                await DisplayAlert("Success", "Registration completed successfully!", "OK");

                // Navigate to LoginPage
                await Navigation.PushAsync(new LoginPage());
            }
            catch (Exception ex)
            {
                // Show error message
                await DisplayAlert("Error", $"Registration failed: {ex.Message}", "OK");
            }
        }

        private async void OnLoginTapped(object sender, EventArgs e)
        {
            // Navigate to LoginPage
            await Navigation.PushAsync(new LoginPage());
        }
    }
}
