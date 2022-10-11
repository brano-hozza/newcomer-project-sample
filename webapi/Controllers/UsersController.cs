using System.Collections.Generic;
using System.Linq;
using System;
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
                cfg.CreateMap<PositionChange, PositionChangeDTO>().ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.Position.Id));
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
            return await db.Users.Include(u => u.Position).Where(user => user.ResignedDate == null).Select(user => mapper.Map<UserDTO>(user)).ToArrayAsync();
        }

        // GET: api/Users/old
        [HttpGet("old")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetOldUsers()
        {
            if (db.Users == null)
            {
                return NotFound();
            }
            return await db.Users.Include(u => u.Position).Where(user => user.ResignedDate != null).Select(user => mapper.Map<UserDTO>(user)).ToArrayAsync();
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
                dto.Id = id;
            }

            return dto ?? (ActionResult<UserDTO>)NotFound();
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDTO dto)
        {
            User? user = await db.Users.Include(u => u.Position).Where(u => u.Id == id).FirstOrDefaultAsync();
            Position? position = await db.Positions.FindAsync(dto.Position);

            if (user == null || position == null)
            {
                return NotFound();
            }

            // Check if position changed
            if (user.Position.Id != dto.Position)
            {
                // Get last position
                PositionChange? change = db.PositionChanges.Where(pc => pc.User.Id == id).OrderByDescending(pc => pc.StartDate).FirstOrDefault();
                if (change == null)
                {
                    return BadRequest();
                }
                // Set end date
                change.EndDate = DateTime.Now;
                db.Entry(change).State = EntityState.Modified;
                // Add new position change
                PositionChange newChange = new()
                {
                    StartDate = DateTime.Now,
                    User = user,
                    Position = position
                };
                db.PositionChanges.Add(newChange);
            }
            // Update user info
            db.Entry(user).State = EntityState.Modified;
            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.BirthDate = dto.BirthDate;
            user.Position = position;
            user.Salary = dto.Salary;

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
            User user = new()
            {
                Name = dto.Name,
                Surname = dto.Surname,
                BirthDate = dto.BirthDate,
                StartDate = dto.StartDate,
                Position = position,
                Salary = dto.Salary
            };
            var newUser = db.Users.Add(user);

            // Create position change
            db.PositionChanges.Add(new() { Position = position, User = newUser.Entity, StartDate = dto.StartDate });
            db.SaveChanges();

            return CreatedAtAction("GetUser", new { id = newUser.Entity.Id }, dto);
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
                user.ResignedDate = System.DateTime.Now;
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

        // GET: api/Users/5/history
        [HttpGet("{id}/history")]
        public async Task<ActionResult<IEnumerable<PositionChangeDTO>>> GetPositionChanges(int id)
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
            return await db.PositionChanges.Include(pc => pc.Position).Where(pc => pc.User.Id == id).Select(pc => mapper.Map<PositionChangeDTO>(pc)).ToArrayAsync();
        }
    }
}
