using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderBoardController : ControllerBase
    {
        private readonly BackendContext _context;

        public LeaderBoardController(BackendContext context)
        {
            _context = context;
        }

        [HttpGet]
        
        public async Task<IActionResult> Get()
        {
            var canon = await _context.CanonChar.ToListAsync();

            var realUsers = await _context.Users.ToListAsync();

            if(realUsers.Count > 0)
            {
                foreach (var user in realUsers)
                {
                    canon.Add(new CanonChar
                    {
                        Name = user.UserName,
                        CanonPL = user.PowerLevel
                    });
                }
            }

            canon = canon.OrderByDescending(x => x.CanonPL).ToList();

            return Ok(canon);
        }
    }
}
