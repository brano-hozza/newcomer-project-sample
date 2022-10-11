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
    public class PositionsController : ControllerBase
    {
        private readonly DataContext db;

        public PositionsController(DataContext context)
        {
            db = context;
        }

        // GET: api/Positions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Position>>> GetPositions()
        {
            if (db.Positions == null)
            {
                return NotFound();
            }
            return await db.Positions.ToListAsync();
        }

        // GET: api/Positions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Position>> GetPosition(int id)
        {
            if (db.Positions == null)
            {
                return NotFound();
            }
            var position = await db.Positions.FindAsync(id);

            return position ?? (ActionResult<Position>)NotFound();
        }

        // POST: api/Positions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Position>> PostPosition(Position position)
        {
            if (db.Positions == null)
            {
                return Problem("Entity set 'DataContext.Positions'  is null.");
            }
            db.Positions.Add(position);
            await db.SaveChangesAsync();

            return CreatedAtAction("GetPosition", new { id = position.Id }, position);
        }

        // DELETE: api/Positions/5
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
            try
            {
                db.Positions.Remove(position);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Conflict();
            }

            return NoContent();
        }
    }
}
