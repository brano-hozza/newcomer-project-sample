using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext db;

        public UsersController(DataContext context)
        {
            db = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            if (db.Users == null)
            {
                return NotFound();
            }
            return await db.Users.Select(user => new UserDTO
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
            if (db.Users == null)
            {
                return NotFound();
            }
            //Include positions with user
            var user = await db.Users.Include(u => u.Position).Where(u => u.Id == id).FirstOrDefaultAsync();
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

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
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
            if (db.Users == null)
            {
                return Problem("Entity set 'DataContext.Users'  is null.");
            }
            var position = await db.Positions.FindAsync(user.Position);
            if (position == null)
            {
                return Problem("Position not found.");
            }
            User newUser = new()
            {
                Name = user.Name,
                Position = position,
                Surname = user.Surname,
                Address = user.Address ?? "",
                Salary = user.Salary,
                BirthDate = user.BirthDate,
                StartDate = user.StartDate
            };
            db.Users.Add(newUser);
            await db.SaveChangesAsync();
            user.Id = newUser.Id;

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (db.Users == null)
            {
                return NotFound();
            }
            var user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (db.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
