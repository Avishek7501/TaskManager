namespace TaskManagerAPI.Models
{
    public class UserTask
    {
        public int TaskId { get; set; }  // Primary Key
        public int UserId { get; set; }  // Foreign Key (Reference to User)
        public string Title { get; set; } = string.Empty; // Task Title
        public string Description { get; set; } = string.Empty; // Task Description (Optional)
        public string Priority { get; set; } = string.Empty; // Task Priority (Low, Medium, High)
        public string Status { get; set; } = string.Empty; // Task Status (Pending, In Progress, Completed)
        public DateTime? DueDate { get; set; } // Task Due Date (Optional)
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Default to current date
    }
}
