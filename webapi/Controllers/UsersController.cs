using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.DTOs;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    /**
    * Controller to process all user operations
    */
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUserService _userService;

        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        // GET: api/users
        // Get all active users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var result = await _userService.GetAll();
            if (result.IsFailed)
            {
                var error = result.Errors[0];
                return BadRequest(error.Exception.Message);
            }
            return result.Value;
        }

        // GET: api/users/old
        // Get all inactive users
        [HttpGet("old")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetOldUsers()
        {
            var result = await _userService.GetAllOld();
            if (result.IsFailed)
            {
                var error = result.Errors[0];
                return BadRequest(error.Exception.Message);
            }
            return result.Value;
        }

        // GET: api/users/5
        // Get specific user data by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var result = await _userService.Get(id);
            if (result.IsFailed)
            {
                var error = result.Errors[0];
                if (error.Type == ErrorType.UserNotFound)
                {
                    return NotFound(error.Exception.Message);
                }
                return BadRequest(error.Exception.Message);
            }
            return result.Value;
        }

        // PUT: api/users/5
        // Update user data based on ID
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }
            var result = await _userService.Update(id, dto);
            if (result.IsFailed)
            {
                var error = result.Errors[0];
                if (error.Type == ErrorType.UserNotFound)
                {
                    return NotFound(error.Exception.Message);
                }
                return BadRequest(error.Exception.Message);

            }
            return NoContent();
        }

        // POST: api/users
        // Create new user based on provided DTO
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserDTO dto)
        {
            var result = await _userService.Create(dto);
            if (result.IsFailed)
            {
                var error = result.Errors[0];
                if (error.Type == ErrorType.UserExists)
                {
                    return Conflict(error.Exception.Message);
                }
                if (error.Type == ErrorType.UserNotFound)
                {
                    return NotFound(error.Exception.Message);
                }
                return BadRequest(error.Exception.Message);
            }
            return CreatedAtAction("GetUser", new { id = result.Value.Id }, result.Value);
        }

        // DELETE: api/users/5/soft
        // Soft delete for setting resignation date
        [HttpDelete("{id}/soft")]
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            var result = await _userService.DeleteSoft(id);
            if (result.IsFailed)
            {
                var error = result.Errors[0];
                if (error.Type == ErrorType.UserNotFound)
                {
                    return NotFound(error.Exception.Message);
                }
                return BadRequest(error.Exception.Message);
            }
            return NoContent();
        }
        // DELETE: api/users/5
        // Hard delete to remove user from DB
        [HttpDelete("{id}")]
        public async Task<IActionResult> NormalDeleteUser(int id)
        {
            var result = await _userService.Delete(id);
            if (result.IsFailed)
            {
                var error = result.Errors[0];
                if (error.Type == ErrorType.UserNotFound)
                {
                    return NotFound(error.Exception.Message);
                }
                return BadRequest(error.Exception.Message);
            }
            return NoContent();
        }

        // GET: api/users/5/history
        // Get specific user history of position change
        [HttpGet("{id}/history")]
        public async Task<ActionResult<IEnumerable<PositionChangeDTO>>> GetPositionChanges(int id)
        {
            var result = await _userService.GetPositionChanges(id);
            if (result.IsFailed)
            {
                var error = result.Errors[0];
                if (error.Type == ErrorType.UserNotFound)
                {
                    return NotFound(error.Exception.Message);
                }
                return BadRequest(error.Exception.Message);
            }
            return result.Value;
        }
    }
}
