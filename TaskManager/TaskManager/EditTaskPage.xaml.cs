using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Model;
using TaskManager.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditTaskPage : ContentPage
    {
        private readonly ApiService _apiService = new ApiService();
        private readonly UserTask _task;

        public EditTaskPage(UserTask task)
        {
            InitializeComponent();
            _task = task;
            BindingContext = _task; // Bind the task to the page
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Get updated details from input fields
            _task.Title = TitleEntry.Text?.Trim();
            _task.Description = DescriptionEditor.Text?.Trim();
            _task.Priority = PriorityPicker.SelectedItem?.ToString();
            _task.Status = StatusPicker.SelectedItem?.ToString();
            _task.DueDate = DueDatePicker.Date;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(_task.Title) || string.IsNullOrWhiteSpace(_task.Priority) || string.IsNullOrWhiteSpace(_task.Status))
            {
                await DisplayAlert("Error", "Please fill all required fields.", "OK");
                return;
            }

            try
            {
                // Call API to update task
                await _apiService.PutAsync($"Task/{_task.TaskId}", _task);
                await DisplayAlert("Success", "Task updated successfully.", "OK");
                // Navigate back to the task details page or list
                await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to update task: {ex.Message}", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            // Navigate back without saving changes
            await Navigation.PopModalAsync();
        }
    }
}