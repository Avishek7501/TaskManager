namespace TaskManagerAPI.Models
{
    public class TaskHistory
    {
        public int HistoryId { get; set; }  // Primary Key
        public int TaskId { get; set; }  // Foreign Key (Reference to Task)
        public string Status { get; set; } = string.Empty; // Status Update (e.g., Pending, Completed)
        public DateTime UpdatedDate { get; set; } = DateTime.Now; // Default to current date
    }
}
