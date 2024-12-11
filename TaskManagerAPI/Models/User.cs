namespace TaskManagerAPI.Models
{
    public class User
    {
        public int UserId { get; set; }  // Primary Key
        public string Name { get; set; } = string.Empty; // User's Name
        public string Email { get; set; } = string.Empty; // User's Email
        public string Password { get; set; } = string.Empty; // User's Password
    }
}
