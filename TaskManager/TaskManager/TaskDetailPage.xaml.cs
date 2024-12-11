using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Model;
using TaskManager.Services;
using TaskManager.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskDetailPage : ContentPage
    {
        private readonly ApiService _apiService = new ApiService();
        private readonly TaskDetailsViewModel _viewModel;

        public TaskDetailPage(UserTask task)
        {
            InitializeComponent();

            if (task == null)
            {
                throw new ArgumentNullException(nameof(task), "Task cannot be null.");
            }

            _viewModel = new TaskDetailsViewModel(task);
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadTaskHistory();
        }

        private async Task LoadTaskHistory()
        {
            try
            {
                var history = await _apiService.GetAsync<List<TaskHistory>>($"Task/{_viewModel.Task.TaskId}/History");
                _viewModel.TaskHistory.Clear();

                if (history != null)
                {
                    foreach (var record in history)
                    {
                        _viewModel.TaskHistory.Add(record);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load task history: {ex.Message}", "OK");
            }
        }
        private async void OnEditTaskClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new EditTaskPage(_viewModel.Task));
        }

        private async void OnDeleteTaskClicked(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Confirm", "Are you sure you want to delete this task?", "Yes", "No");
            if (confirm)
            {
                try
                {
                    await _apiService.DeleteAsync($"Tasks/{_viewModel.Task.TaskId}");
                    await DisplayAlert("Success", "Task deleted successfully.", "OK");

                    // Navigate back to the task list
                    await Navigation.PopModalAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to delete task: {ex.Message}", "OK");
                }
            }
        }
    }
}