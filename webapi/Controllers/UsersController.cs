using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext db;
        private readonly IMapper mapper;

        public UsersController(DataContext _db, IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            if (db.Users == null)
            {
                return NotFound();
            }
            return await db.Users.Include(u => u.Position).Select(user => mapper.Map<UserDTO>(user)).ToArrayAsync();
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
                dto = mapper.Map<UserDTO>(user);
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
        public async Task<ActionResult<User>> PostUser(UserDTO dto)
        {
            if (db.Users == null)
            {
                return Problem("Entity set 'DataContext.Users'  is null.");
            }
            var position = await db.Positions.FindAsync(dto.Position);
            if (position == null)
            {
                return Problem("Position not found.");
            }
            User user = mapper.Map<User>(dto);
            db.Users.Add(user);
            await db.SaveChangesAsync();
            dto.Id = user.Id;

            return CreatedAtAction("GetUser", new { id = dto.Id }, dto);
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
