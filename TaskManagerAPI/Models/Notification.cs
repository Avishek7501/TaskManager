namespace TaskManagerAPI.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }  // Primary Key
        public int UserId { get; set; }  // Foreign Key (Reference to User)
        public int TaskId { get; set; }  // Foreign Key (Reference to Task)
        public string Message { get; set; } = string.Empty; // Notification Message
        public DateTime NotificationDate { get; set; } // Date of Notification
        public bool IsRead { get; set; } = false; // Default to unread
    }
}
