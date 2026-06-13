using Backend.Models;
using Backend.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Authorize] // Locks down the entire controller to logged-in Z-Fighters
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly BackendContext _context;
        private readonly UserManager<AppUser> _userManager;

        public TaskController(BackendContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Helper method to get the currently logged-in user's ID from the JWT
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // GET: api/task
        // Fetches all active training tasks for the user
        [HttpGet]
        public async Task<IActionResult> GetMyTasks()
        {
            var userId = GetUserId();
            var tasks = await _context.TodoTasks
                .Where(t => t.UserId == userId && !t.IsCompleted)
                .OrderBy(t => t.CreatedAt)
                .ToListAsync();

            return Ok(tasks);
        }

        // POST: api/task
        // Adds a new task to your training regimen
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
        {
            var userId = GetUserId();

            // Calculate the Power Level reward based on the tier
            // Daily = 50 PL, Weekly = 500 PL
            int calculatedReward = dto.TaskTier == 1 ? 500 : 50;

            var newTask = new TodoTask
            {
                UserId = userId,
                Title = dto.Title,
                TaskTier = dto.TaskTier,
                RewardPL = calculatedReward,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.TodoTasks.Add(newTask);
            await _context.SaveChangesAsync();

            return Ok(newTask);
        }

        // PUT: api/task/{id}/complete
        // The most important endpoint: Finishes a task and boosts your Power Level
        [HttpPut("{id}/complete")]
        public async Task<IActionResult> CompleteTask(int id)
        {
            var userId = GetUserId();

            // 1. Find the task and ensure it belongs to the current user
            var task = await _context.TodoTasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null) return NotFound("Task not found.");
            if (task.IsCompleted) return BadRequest("This task is already completed.");

            // 2. Mark as complete
            task.IsCompleted = true;

            // 3. Find the user to update their stats
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return Unauthorized();

            // 4. Boost Power Level
            user.PowerLevel += task.RewardPL;

            // 5. Check for Transformations (Zenkai/Milestones)
            string transformationMessage = CheckForTransformations(user);

            // 6. Save all changes (Task update + User stat update)
            await _context.SaveChangesAsync();
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                Message = $"Task completed! +{task.RewardPL} PL.",
                NewPowerLevel = user.PowerLevel,
                Alert = transformationMessage
            });
        }

        // DELETE: api/task/{id}
        // Drops a task from the list
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = GetUserId();
            var task = await _context.TodoTasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null) return NotFound();

            _context.TodoTasks.Remove(task);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Task removed from your training regimen." });
        }

        // --- Core Business Logic ---

        // Evaluates if the user's new Power Level unlocks a new form
        private string CheckForTransformations(AppUser user)
        {
            // Form Enums: 0 = Base, 1 = Kaioken, 2 = Super Saiyan

            if (user.PowerLevel >= 1000000 && user.CurrentForm < 2)
            {
                user.CurrentForm = 2;
                return "TRANSFORMATION UNLOCKED: You have ascended to Super Saiyan!";
            }
            else if (user.PowerLevel >= 10000 && user.CurrentForm < 1)
            {
                user.CurrentForm = 1;
                return "TRANSFORMATION UNLOCKED: You can now use the Kaioken technique!";
            }

            return null; // No new transformation
        }
    }
}