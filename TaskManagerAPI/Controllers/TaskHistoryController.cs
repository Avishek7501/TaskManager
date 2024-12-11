using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Models;
using System;
using System.Threading.Tasks;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskHistoryController : ControllerBase
    {
        private readonly TaskManagerDbContext _context;

        public TaskHistoryController(TaskManagerDbContext context)
        {
            _context = context;
        }

        // GET: api/TaskHistory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskHistory>>> GetTaskHistories()
        {
            return await _context.TaskHistories.ToListAsync();
        }


       
    }
}
