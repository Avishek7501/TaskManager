using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskManagerDbContext _context;

        public TaskController(TaskManagerDbContext context)
        {
            _context = context;
        }

        // GET: api/Task
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTask>>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        // GET: api/Task/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTask>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound(new { Message = "Task not found." });
            }
            return Ok(task);
        }

        // GET: api/Task/User/{userId}
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<UserTask>>> GetTasksByUser(int userId)
        {
            // Fetch tasks for the given userId
            var tasks = await _context.Tasks
                .Where(t => t.UserId == userId)
                .ToListAsync();

            // Return an empty list instead of a 404 when no tasks are found
            if (tasks == null || tasks.Count == 0)
            {
                return Ok(new List<UserTask>());
            }

            return Ok(tasks); // Return the list of tasks
        }

        // POST: api/Task
        [HttpPost]
        public async Task<ActionResult<UserTask>> CreateTask(UserTask task)
        {
            task.CreatedDate = DateTime.Now;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.TaskId }, task);
        }

        // PUT: api/Task/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, UserTask task)
        {
            if (id != task.TaskId)
            {
                return BadRequest(new { Message = "Task ID mismatch." });
            }

            var existingTask = await _context.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                return NotFound(new { Message = "Task not found." });
            }

            var taskHistory = new TaskHistory
            {
                TaskId = task.TaskId,
                Status = task.Status,
                UpdatedDate = DateTime.Now
            };
            _context.TaskHistories.Add(taskHistory);


            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Priority = task.Priority;
            existingTask.Status = task.Status;
            existingTask.DueDate = task.DueDate;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Task/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound(new { Message = "Task not found." });
            }

            // Delete related notifications
            var notifications = await _context.Notifications.Where(n => n.TaskId == id).ToListAsync();
            _context.Notifications.RemoveRange(notifications);

            // Delete related task histories
            var histories = await _context.TaskHistories.Where(th => th.TaskId == id).ToListAsync();
            _context.TaskHistories.RemoveRange(histories);

            // Delete the task
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Task/{taskId}/History
        [HttpGet("{taskId}/History")]
        public async Task<ActionResult<IEnumerable<TaskHistory>>> GetTaskHistory(int taskId)
        {
            var taskHistory = await _context.TaskHistories
                .Where(h => h.TaskId == taskId)
                .OrderByDescending(h => h.UpdatedDate)
                .ToListAsync();

            if (!taskHistory.Any())
            {
                return NotFound("No history found for this task.");
            }

            return Ok(taskHistory);
        }
    }
}
