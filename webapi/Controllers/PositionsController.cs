using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
   * Controller to process all position operations
   */
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IPositionService _positionService;

        public PositionsController(ILogger<PositionsController> logger, IPositionService positionService)
        {
            this._logger = logger;
            this._positionService = positionService;
        }

        // GET: api/positions
        // Return all positions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PositionDTO>>> GetPositions()
        {
            var result = await _positionService.GetAll();
            if (result.IsFailed)
            {
                var error = result.Errors[0];
                return BadRequest(error.Exception.Message);
            }
            return result.Value;
        }

        // POST: api/positions
        // Create new position
        [HttpPost]
        public async Task<ActionResult<PositionDTO>> PostPosition(PositionDTO position)
        {
            _logger.LogInformation("Creating new position", position.Name);
            var result = await _positionService.Create(position);
            if (result.IsFailed)
            {
                var error = result.Errors[0];
                return BadRequest(error.Exception.Message);
            }
            _logger.LogInformation("Created new position {Name}", position.Name);

            return result.Value;
        }

        // DELETE: api/positions/5
        // Delete position with specified ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            var result = await _positionService.Delete(id);
            if (result.IsFailed)
            {
                var error = result.Errors[0];
                if (error.Type == ErrorType.PositionNotFound)
                {
                    return NotFound(error.Exception.Message);
                }
                return BadRequest(error.Exception.Message);
            }
            return NoContent();
        }
    }
}
