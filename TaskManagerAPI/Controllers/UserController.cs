using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TaskManagerDbContext _context;

        public UserController(TaskManagerDbContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            return Ok(user);
        }

        // POST: api/User/Register
        [HttpPost("Register")]
        public async Task<ActionResult<User>> RegisterUser(User user)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                return Conflict(new { Message = "A user with this email already exists." });
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            // Find all tasks created by the user
            var tasks = await _context.Tasks.Where(t => t.UserId == id).ToListAsync();

            foreach (var task in tasks)
            {
                // Delete associated notifications
                var notifications = await _context.Notifications.Where(n => n.TaskId == task.TaskId).ToListAsync();
                _context.Notifications.RemoveRange(notifications);

                // Delete associated task histories
                var histories = await _context.TaskHistories.Where(th => th.TaskId == task.TaskId).ToListAsync();
                _context.TaskHistories.RemoveRange(histories);
            }

            // Delete tasks
            _context.Tasks.RemoveRange(tasks);

            // Delete the user
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
