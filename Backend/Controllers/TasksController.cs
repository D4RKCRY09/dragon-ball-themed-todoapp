using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly BackendContext _context;
    public TasksController(BackendContext context)
    {
        _context = context;
    }

    // GET: api/TodoTask
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoTask>>> GetTodoTask()
    {
        return await _context.TodoTask.ToListAsync();
    }

    // GET: api/TodoTask/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoTask>> GetTodoTask(int id)
    {
        var todotask = await _context.TodoTask.FindAsync(id);

        if (todotask == null)
        {
            return NotFound();
        }

        return todotask;
    }

    // PUT: api/TodoTask/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoTask(int? id, TodoTask todotask)
    {
        if (id != todotask.Id)
        {
            return BadRequest();
        }

        _context.Entry(todotask).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TodoTaskExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/TodoTask
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TodoTask>> PostTodoTask(TodoTask todotask)
    {
        _context.TodoTask.Add(todotask);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTodoTask", new { id = todotask.Id }, todotask);
    }

    // DELETE: api/TodoTask/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoTask(int? id)
    {
        var todotask = await _context.TodoTask.FindAsync(id);
        if (todotask == null)
        {
            return NotFound();
        }

        _context.TodoTask.Remove(todotask);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TodoTaskExists(int? id)
    {
        return _context.TodoTask.Any(e => e.Id == id);
    }
}
