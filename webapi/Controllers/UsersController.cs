using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.Models;

namespace WebApi.Controllers
{
    /**
    * Controller to process all user operations
    */
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext db;
        private readonly ILogger _logger;
        private readonly IMapper mapper;

        public UsersController(DataContext _db, ILogger<UsersController> logger)
        {
            db = _db;
            // Configure automapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>().ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position.Id));
                cfg.CreateMap<UserDTO, User>().ForMember(dest => dest.Position, opt => opt.MapFrom(src => db.Positions.FirstOrDefault(p => p.Id == src.Position)));
                cfg.CreateMap<Position, PositionDTO>();
                cfg.CreateMap<PositionChange, PositionChangeDTO>().ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.Position!.Id));
            });
            mapper = new Mapper(config);
            _logger = logger;
        }

        // GET: api/users
        // Get all active users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            if (db.Users == null)
            {
                return NotFound();
            }
            // Filter out only users without resignation date
            return await db.Users.Include(u => u.Position).Where(user => user.ResignedDate == null).Select(user => mapper.Map<UserDTO>(user)).ToArrayAsync();
        }

        // GET: api/users/old
        // Get all inactive users
        [HttpGet("old")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetOldUsers()
        {
            if (db.Users == null)
            {
                return NotFound();
            }
            // Filter out only users with resignation date
            return await db.Users.Include(u => u.Position).Where(user => user.ResignedDate != null).Select(user => mapper.Map<UserDTO>(user)).ToArrayAsync();
        }

        // GET: api/users/5
        // Get specific user data by ID
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

            _logger.LogInformation("Getting user ID: {id1} by ID : {id2}", dto.Id, id);

            return dto ?? (ActionResult<UserDTO>)NotFound();
        }

        // PUT: api/users/5
        // Update user data based on ID
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDTO dto)
        {
            User? user = await db.Users.Include(u => u.Position).Where(u => u.Id == id).FirstOrDefaultAsync();
            Position? position = await db.Positions.FindAsync(dto.Position);

            //Check user and position existence
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

            _logger.LogInformation("Updated user with ID:{Id}", user.Id);

            return NoContent();
        }

        // POST: api/users
        // Create new user based on provided DTO
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserDTO dto)
        {
            if (db.Users == null)
            {
                return Problem("Entity set 'DataContext.Users'  is null.");
            }
            var position = await db.Positions.FindAsync(dto.Position);
            // Check position existence
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
            db.Users.Add(user);

            // Create position change
            db.PositionChanges.Add(new() { Position = position, User = user, StartDate = dto.StartDate });
            db.SaveChanges();
            _logger.LogInformation("Created user with ID:{Id}", user.Id);
            dto.Id = user.Id;
            return CreatedAtAction("GetUser", new { id = user.Id }, dto);
        }

        // DELETE: api/users/5/soft
        // Soft delete for setting resignation date
        [HttpDelete("{id}/soft")]
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            return await DeleteUser(id, true);
        }
        // DELETE: api/users/5
        // Hard delete to remove user from DB
        [HttpDelete("{id}")]
        public async Task<IActionResult> NormalDeleteUser(int id)
        {
            return await DeleteUser(id);
        }

        /**
        * Method to delete user
        * Set soft=true for soft delete
        */
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
        // Check for user existence
        private bool UserExists(int id)
        {
            return (db.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // GET: api/users/5/history
        // Get specific user history of position change
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
