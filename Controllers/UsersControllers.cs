using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokeBid.API.Data;
using PokeBid.API.DTOs;
using PokeBid.API.Models;

namespace PokeBid.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly PokeBidDbContext _context;

        public UsersController(PokeBidDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users.Select(u => new UserDto { Id = u.Id, Username = u.Username, Email = u.Email }).ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return Ok(new UserDto { Id = user.Id, Username = user.Username, Email = user.Email });
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto dto)
        {
            var user = new User { Username = dto.Username, Email = dto.Email, PasswordHash = dto.Password };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return StatusCode(201, new UserDto { Id = user.Id, Username = user.Username, Email = user.Email });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}