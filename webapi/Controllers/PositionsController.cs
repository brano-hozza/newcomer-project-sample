using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi.Models;

namespace WebApi.Controllers
{
    /**
   * Controller to process all position operations
   */
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly DataContext db;
        private readonly ILogger _logger;

        public PositionsController(DataContext context, ILogger<PositionsController> logger)
        {
            db = context;
            this._logger = logger;
        }

        // GET: api/positions
        // Return all positions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Position>>> GetPositions()
        {
            if (db.Positions == null)
            {
                return NotFound();
            }
            return await db.Positions.ToListAsync();
        }

        // POST: api/positions
        // Create new position
        [HttpPost]
        public async Task<ActionResult<Position>> PostPosition(Position position)
        {
            if (db.Positions == null)
            {
                return Problem("Entity set 'DataContext.Positions'  is null.");
            }
            db.Positions.Add(position);
            await db.SaveChangesAsync();

            _logger.LogInformation("Created new position {Name}", position.Name);

            return CreatedAtAction("GetPosition", new { id = position.Id }, position);
        }

        // DELETE: api/positions/5
        // Delete position with specified ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            if (db.Positions == null)
            {
                return NotFound();
            }
            var position = await db.Positions.FindAsync(id);
            if (position == null)
            {
                return NotFound();
            }
            try // Handle reference error
            {
                db.Positions.Remove(position);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                _logger.LogError("Unsucesful try to delete position with existing references, ID:{Id}", id);
                return Conflict();
            }

            return NoContent();
        }
    }
}
