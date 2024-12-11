using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TaskManagerDbContext _context;

        public AuthController(TaskManagerDbContext context)
        {
            _context = context;
        }

        // POST: api/Auth/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _context.Users
                .Where(m => m.Email == loginRequest.Email && m.Password == loginRequest.Password)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return Unauthorized(); // Return 401 if login fails
            }

            return Ok(user); // Return 200 with member details if successful
        }
    }
}
