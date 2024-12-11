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
    public partial class CreateTaskPage : ContentPage
    {
        private readonly ApiService _apiService = new ApiService(); // Initialize API service
        private readonly int _userId;

        public CreateTaskPage(int userId)
        {
            InitializeComponent();
            _userId = userId;
        }

        private async void OnCreateTaskClicked(object sender, EventArgs e)
        {
            var title = TitleEntry.Text?.Trim();
            var description = DescriptionEditor.Text?.Trim();
            var priority = PriorityPicker.SelectedItem?.ToString();
            var dueDate = DueDatePicker.Date;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(title))
            {
                await DisplayAlert("Error", "Task title is required.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(priority))
            {
                await DisplayAlert("Error", "Please select a priority.", "OK");
                return;
            }

            try
            {
                // Create task object
                var newTask = new UserTask
                {
                    UserId = _userId,
                    Title = title,
                    Description = description,
                    Priority = priority,
                    DueDate = dueDate,
                    CreatedDate = DateTime.Now,
                    Status = "Pending" // Default status for a new task
                };

                // Call API to create the task
                await _apiService.PostAsync<UserTask>("Task", newTask);

                await DisplayAlert("Success", "Task created successfully!", "OK");

                // Navigate back to the previous page
                await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to create task: {ex.Message}", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            // Navigate back to the previous page without saving
            await Navigation.PopModalAsync();
        }
    }
}