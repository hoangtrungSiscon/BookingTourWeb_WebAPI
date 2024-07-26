using BookingTourWeb_WebAPI.Models.InputModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Globalization;
using System.Numerics;




namespace BookingTourWeb_WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class KhachHangsController : ControllerBase
    {
        private readonly DVMayBayContext _context;

        public KhachHangsController(DVMayBayContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<ThongTinKhachHang>>> GetThongTinKhachHang()
        {
            var ThongTinKhachHang = (from khachhang in _context.Khachhangs
                                     select new ThongTinKhachHang()
                                     {
                                         Makhachhang = khachhang.MaKh,
                                         MaTaiKhoan = khachhang.MaKh,
                                         HoTenKh = khachhang.TenKh,
                                         Phai = khachhang.Phai,
                                         GmailKh = khachhang.GmailKh,
                                         Sdt = khachhang.Sdt
                                     })
            .ToListAsync();

            return await ThongTinKhachHang;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Khachhang>> GetKhachHang(long id)
        {
            if (_context.Khachhangs == null)
            {
                return NotFound();
            }
            var khachhang = await _context.Khachhangs.FindAsync(id);

            if (khachhang == null)
            {
                return NotFound();
            }

            return khachhang;
        }

        


        [HttpPut("{id}")]
        public async Task<IActionResult> PutKhachHang(long id, Khachhang khachhang)
        {
            if (id != khachhang.MaKh)
            {
                return BadRequest();
            }

            _context.Entry(khachhang).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KhachHangExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Khachhang>> PostKhachhang(Khachhang khachhang)
        {
            if (_context.Khachhangs == null)
            {
                return Problem("Entity set 'DVMayBayContext.Khachhangs'  is null.");
            }
            _context.Khachhangs.Add(khachhang);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (KhachHangExists(khachhang.MaKh))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetKhachhang", new { id = khachhang.MaKh }, khachhang);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKhachhang(long id)
        {
            if (_context.Khachhangs == null)
            {
                return NotFound();
            }
            var khachhang = await _context.Khachhangs.FindAsync(id);
            if (khachhang == null)
            {
                return NotFound();
            }

            _context.Khachhangs.Remove(khachhang);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KhachHangExists(long id)
        {
            return (_context.Khachhangs?.Any(e => e.MaKh == id)).GetValueOrDefault();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Khachhang>> GetByMaTaiKhoanAsync(long id)
        {
            if (_context.Khachhangs == null)
            {
                return NotFound();
            }
            var khachhang = await _context.Khachhangs.Where(x => x.MaTaiKhoan == id).FirstOrDefaultAsync();

            if (khachhang == null)
            {
                return NotFound();
            }

            return Ok(khachhang);
        }
    }
}
