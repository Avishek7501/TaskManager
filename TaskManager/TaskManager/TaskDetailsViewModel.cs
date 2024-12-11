using System;
using System.Collections.ObjectModel;
using TaskManager.Model;

namespace TaskManager.ViewModels
{
    public class TaskDetailsViewModel
    {
        public UserTask Task { get; set; }
        public ObservableCollection<TaskHistory> TaskHistory { get; set; }

        public TaskDetailsViewModel(UserTask task)
        {
            Task = task;
            TaskHistory = new ObservableCollection<TaskHistory>();
        }
    }
}
