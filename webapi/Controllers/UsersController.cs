using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            return await _context.Users.Select(user => new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Address = user.Address,
                BirthDate = user.BirthDate,
                StartDate = user.StartDate,
                Salary = user.Salary,
                Position = user.Position.Id
            }).ToArrayAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            //Include positions with user
            var user = await _context.Users.Include(u => u.Position).Where(u => u.Id == id).FirstOrDefaultAsync();
            UserDTO dto = null!;
            if (user != null)
            {
                dto = new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Address = user.Address,
                    BirthDate = user.BirthDate,
                    StartDate = user.StartDate,
                    Salary = user.Salary,
                    Position = user.Position.Id
                };
            }

            return dto ?? (ActionResult<UserDTO>)NotFound();
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!UserExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserDTO user)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'DataContext.Users'  is null.");
            }
            var position = await _context.Positions.FindAsync(user.Position);
            if (position == null)
            {
                return Problem("Position not found.");
            }
            User newUser = new User
            {
                Name = user.Name,
                Position = position,
                Surname = user.Surname,
                Address = user.Address ?? "",
                Salary = user.Salary,
                BirthDate = user.BirthDate,
                StartDate = user.StartDate
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            user.Id = newUser.Id;

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
