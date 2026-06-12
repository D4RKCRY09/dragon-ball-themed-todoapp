using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;

[Route("api/[controller]")]
[ApiController]
public class LeaderboardController : ControllerBase
{
    private readonly BackendContext _context;
    public LeaderboardController(BackendContext context)
    {
        _context = context;
    }

    // GET: api/CanonChar
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CanonChar>>> GetCanonChar()
    {
        return await _context.CanonChar.ToListAsync();
    }

    // GET: api/CanonChar/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CanonChar>> GetCanonChar(int id)
    {
        var canonchar = await _context.CanonChar.FindAsync(id);

        if (canonchar == null)
        {
            return NotFound();
        }

        return canonchar;
    }

    // PUT: api/CanonChar/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCanonChar(int? id, CanonChar canonchar)
    {
        if (id != canonchar.Id)
        {
            return BadRequest();
        }

        _context.Entry(canonchar).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CanonCharExists(id))
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

    // POST: api/CanonChar
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<CanonChar>> PostCanonChar(CanonChar canonchar)
    {
        _context.CanonChar.Add(canonchar);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCanonChar", new { id = canonchar.Id }, canonchar);
    }

    // DELETE: api/CanonChar/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCanonChar(int? id)
    {
        var canonchar = await _context.CanonChar.FindAsync(id);
        if (canonchar == null)
        {
            return NotFound();
        }

        _context.CanonChar.Remove(canonchar);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CanonCharExists(int? id)
    {
        return _context.CanonChar.Any(e => e.Id == id);
    }
}
