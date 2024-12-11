using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class TasksPage : ContentPage
    {
        private readonly ApiService _apiService = new ApiService(); // API service instance
        private readonly int _userId;
        public ObservableCollection<UserTask> Tasks { get; set; }

        public TasksPage(int userId)
        {
            InitializeComponent();
            _userId = userId;
            Tasks = new ObservableCollection<UserTask>();
            TasksCollectionView.ItemsSource = Tasks;
            LoadTasks();
        }


        private async void LoadTasks()
        {
            try
            {
                // Fetch tasks from the API
                var tasks = await _apiService.GetAsync<List<UserTask>>($"Task/User/{_userId}");

                // Clear existing tasks and add the new ones
                Tasks.Clear();
                foreach (var task in tasks)
                {
                    Tasks.Add(task);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load tasks: {ex.Message}", "OK");
            }
        }

        private async void OnTaskSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                var selectedTask = e.CurrentSelection[0] as UserTask;

                // Navigate to the TaskDetailsPage or handle actions
                await Navigation.PushModalAsync(new TaskDetailPage(selectedTask));
            }

            // Deselect the item after action
            ((CollectionView)sender).SelectedItem = null;
        }

        private void OnRefreshClicked(object sender, EventArgs e)
        {
            LoadTasks();
        }
    }
}