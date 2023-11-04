using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TourBookingWeb_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Chuyenbayscontroller : ControllerBase
    {
        /*
        private readonly DVMayBayContext _context;
        public Chuyenbayscontroller(DVMayBayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Chuyenbay>>> GetChuyenbays()
        {
            return Ok(await _context.Chuyenbays
                .ToListAsync());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Chuyenbay>> GetChuyenbay(string id)
        {
            if (_context.Chuyenbays == null)
            {
                return NotFound();
            }
            var Chuyenbay = await _context.Chuyenbays.FindAsync(id);

            if (Chuyenbay == null)
            {
                return NotFound();
            }

            return Chuyenbay;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutChuyenbay(string id, Chuyenbay Chuyenbay)
        {
            if (id != Chuyenbay.MaChuyenBay)
            {
                return BadRequest();
            }

            _context.Entry(Chuyenbay).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChuyenbayExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Chuyenbays
        [HttpPost]
        public async Task<ActionResult<Chuyenbay>> PostChuyenbay(Chuyenbay Chuyenbay)
        {
            if (_context.Chuyenbays == null)
            {
                return Problem("Entity set 'DvmayBayContext.Chuyenbays'  is null.");
            }
            _context.Chuyenbays.Add(Chuyenbay);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ChuyenbayExists(Chuyenbay.MaChuyenBay))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetChuyenbay", new { id = Chuyenbay.MaChuyenBay }, Chuyenbay);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChuyenbay(string id)
        {
            if (_context.Chuyenbays == null)
            {
                return NotFound();
            }
            var Chuyenbay = await _context.Chuyenbays.FindAsync(id);
            if (Chuyenbay == null)
            {
                return NotFound();
            }

            _context.Chuyenbays.Remove(Chuyenbay);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChuyenbayExists(string id)
        {
            return (_context.Chuyenbays?.Any(e => e.MaChuyenBay == id)).GetValueOrDefault();
        }*/
    }
}
