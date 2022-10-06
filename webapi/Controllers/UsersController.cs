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

        public UsersController(DataContext _db)
        {
            db = _db;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>().ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position.Id));
                cfg.CreateMap<UserDTO, User>().ForMember(dest => dest.Position, opt => opt.MapFrom(src => db.Positions.FirstOrDefault(p => p.Id == src.Position)));
                cfg.CreateMap<Position, PositionDTO>();
            });
            mapper = new Mapper(config);
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            if (db.Users == null)
            {
                return NotFound();
            }
            return await db.Users.Include(u => u.Position).Where(user => !user.Resigned).Select(user => mapper.Map<UserDTO>(user)).ToArrayAsync();
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
        public async Task<IActionResult> PutUser(int id, UserDTO dto)
        {
            User? user = await db.Users.Include(u => u.Position).FirstOrDefaultAsync();
            Position? position = await db.Positions.FindAsync(dto.Position);

            if (user == null || position == null)
            {
                return BadRequest();
            }

            if (user.Position.Id != dto.Position)
            {
                PositionChange? change = db.PositionChanges.Where(pc => pc.User.Id == user.Id).OrderByDescending(pc => pc.StartDate).FirstOrDefault();
                if (change == null)
                {
                    return BadRequest();
                }
                change.EndDate = System.DateTime.Now;
                db.Entry(change).State = EntityState.Modified;
                PositionChange newChange = new()
                {
                    StartDate = System.DateTime.Now,
                    User = user,
                    Position = position
                };
                db.PositionChanges.Add(newChange);
            }

            db.Entry(user).State = EntityState.Modified;
            user = mapper.Map(dto, user);

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
            db.PositionChanges.Add(new() { Position = position, User = user, StartDate = System.DateTime.Now });
            await db.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, dto);
        }

        // DELETE: api/Users/5/soft
        [HttpDelete("{id}/soft")]
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            return await DeleteUser(id, true);
        }
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> NormalDeleteUser(int id)
        {
            return await DeleteUser(id);
        }

        private async Task<IActionResult> DeleteUser(int id, bool soft = false)
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
            if (soft)
            {
                user.Resigned = true;
                db.Entry(user).State = EntityState.Modified;
            }
            else
            {
                db.Users.Remove(user);
            }
            await db.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (db.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
