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
        private readonly DataContext _db;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public UsersController(DataContext db, ILogger<UsersController> logger)
        {
            this._db = db;
            // Configure automapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>().ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position.Id));
                cfg.CreateMap<UserDTO, User>().ForMember(dest => dest.Position, opt => opt.MapFrom(src => this._db.Positions.FirstOrDefault(p => p.Id == src.Position)));
                cfg.CreateMap<Position, PositionDTO>();
                cfg.CreateMap<PositionChange, PositionChangeDTO>().ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.Position!.Id));
            });
            _mapper = new Mapper(config);
            _logger = logger;
        }

        // GET: api/users
        // Get all active users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            // Filter out only users without resignation date
            return await _db.Users.Include(u => u.Position).Where(user => user.ResignedDate == null).Select(user => _mapper.Map<UserDTO>(user)).ToArrayAsync();
        }

        // GET: api/users/old
        // Get all inactive users
        [HttpGet("old")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetOldUsers()
        {
            // Filter out only users with resignation date
            return await _db.Users.Include(u => u.Position).Where(user => user.ResignedDate != null).Select(user => _mapper.Map<UserDTO>(user)).ToArrayAsync();
        }

        // GET: api/users/5
        // Get specific user data by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            //Include positions with user
            var user = await _db.Users.Include(u => u.Position).Where(u => u.Id == id).FirstOrDefaultAsync();
            UserDTO dto = null!;
            if (user != null)
            {
                dto = _mapper.Map<UserDTO>(user);
                dto.Id = id;
            }

            _logger.LogInformation("Getting user ID: {id1} by ID : {id2}", dto.Id, id);

            return dto;
        }

        // PUT: api/users/5
        // Update user data based on ID
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDTO dto)
        {
            User? user = await _db.Users.Include(u => u.Position).Where(u => u.Id == id).FirstOrDefaultAsync();
            Position? position = await _db.Positions.FindAsync(dto.Position);

            //Check user and position existence
            if (user == null || position == null)
            {
                return NotFound();
            }

            var _u = await _db.Users.Select(u => u).Where(u => u.Id != dto.Id && u.Name == dto.Name && u.Surname == dto.Surname).FirstOrDefaultAsync();
            if (_u != null)
            {
                return Problem("User already exists.", "User", 403);
            }

            // Check if position changed
            if (user.Position.Id != dto.Position)
            {
                // Get last position
                PositionChange? change = _db.PositionChanges.Where(pc => pc.User.Id == id).OrderByDescending(pc => pc.StartDate).FirstOrDefault();
                if (change == null)
                {
                    return BadRequest();
                }
                // Set end date
                change.EndDate = DateTime.Now;
                _db.Entry(change).State = EntityState.Modified;
                // Add new position change
                PositionChange newChange = new()
                {
                    StartDate = DateTime.Now,
                    User = user,
                    Position = position
                };
                _db.PositionChanges.Add(newChange);
            }
            // Update user info
            _db.Entry(user).State = EntityState.Modified;
            user.Position = position;
            user.Address = dto.Address;
            user.Salary = dto.Salary;

            try
            {
                await _db.SaveChangesAsync();
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
            var position = await _db.Positions.FindAsync(dto.Position);
            // Check position existence
            if (position == null)
            {
                return Problem("Position not found.");
            }
            var _u = await _db.Users.Select(u => u).Where(u => u.Name == dto.Name && u.Surname == dto.Surname).FirstOrDefaultAsync();
            if (_u != null)
            {
                return Problem("User already exists.", "User", 403);
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
            _db.Users.Add(user);

            // Create position change
            _db.PositionChanges.Add(new() { Position = position, User = user, StartDate = dto.StartDate });
            await _db.SaveChangesAsync();
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
            var user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            if (soft)
            {

                var change = await _db.PositionChanges.Where(pc => pc.User.Id == id).OrderByDescending(pc => pc.StartDate).FirstOrDefaultAsync();
                if (change == null)
                {
                    return BadRequest();
                }
                change.EndDate = DateTime.Now;
                user.ResignedDate = DateTime.Now;
                _db.Entry(user).State = EntityState.Modified;
            }
            else
            {
                _db.Users.Remove(user);
            }
            await _db.SaveChangesAsync();

            return NoContent();
        }
        // Check for user existence
        private bool UserExists(int id)
        {
            return _db.Users.Any(e => e.Id == id);
        }

        // GET: api/users/5/history
        // Get specific user history of position change
        [HttpGet("{id}/history")]
        public async Task<ActionResult<IEnumerable<PositionChangeDTO>>> GetPositionChanges(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return await _db.PositionChanges.Include(pc => pc.Position).Where(pc => pc.User.Id == id).Select(pc => _mapper.Map<PositionChangeDTO>(pc)).ToArrayAsync();
        }
    }
}
