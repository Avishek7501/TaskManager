using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly TaskManagerDbContext _context;

        public NotificationController(TaskManagerDbContext context)
        {
            _context = context;
        }

        // GET: api/Notification/User/{userId}
        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetNotificationsByUser(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.NotificationDate)
                .ToListAsync();

            return Ok(notifications);
        }

        // GET: api/Notification/User/{userId}/Unread
        [HttpGet("User/{userId}/Unread")]
        public async Task<IActionResult> GetUnreadNotificationsByUser(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.NotificationDate)
                .ToListAsync();

            return Ok(notifications);
        }

        // PUT: api/Notification/MarkAsRead/{notificationId}
        [HttpPut("MarkAsRead/{notificationId}")]
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);

            if (notification == null)
            {
                return NotFound(new { Message = "Notification not found." });
            }

            notification.IsRead = true;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Notification marked as read." });
        }

        [HttpPost("Generate")]
        public async Task<IActionResult> GenerateNotifications()
        {
            try
            {
                var today = DateTime.Today;
                var threeDaysFromNow = today.AddDays(3);

                // Fetch tasks due within 3 days that are not completed
                var tasksToNotify = await _context.Tasks
                    .Where(t => t.DueDate >= today && t.DueDate < threeDaysFromNow && t.Status != "Completed")
                    .ToListAsync();

                foreach (var task in tasksToNotify)
                {
                    // Check if a notification for this task already exists for today
                    var notificationExists = await _context.Notifications
                        .AnyAsync(n => n.TaskId == task.TaskId && n.NotificationDate == today);

                    if (!notificationExists)
                    {
                        // Create a new notification
                        var notification = new Notification
                        {
                            UserId = task.UserId,
                            TaskId = task.TaskId,
                            Message = $"Reminder: Task '{task.Title}' is due on {task.DueDate:MM/dd/yyyy}.",
                            NotificationDate = today,
                            IsRead = false
                        };

                        _context.Notifications.Add(notification);
                    }
                }

                // Save all notifications
                await _context.SaveChangesAsync();

                return Ok(new { message = "Notifications generated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
