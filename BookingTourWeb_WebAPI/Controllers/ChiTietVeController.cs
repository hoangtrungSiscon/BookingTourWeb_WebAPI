using BookingBackend.Models;
using BookingBackend.Models.InputModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChiTietVeController : ControllerBase
    {
        private readonly DvmayBayContext _context;

        public ChiTietVeController(DvmayBayContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Chitietve>>> GetAllAsync() {
            return await _context.Chitietves.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<List<object>>> FilterChiTietVeAsync(InputFilterGuestsChuyenBay input)
        {
            var filterVe = await _context.Ves.Where(x => x.NgayDatVe >= DateTime.Parse(input.bookDate)).ToListAsync();
            var filterChiTietVes = await _context.Chitietves.Where(x => filterVe.Any(xx => xx.MaVe == x.MaVe)).ToListAsync();
            var ves = filterVe.Join(_context.Khachhangs, x => x.MaKh, khachHang => khachHang.MaKh, (x, khachHang) => new
            {
                MaVe = x.MaVe,
                NgayDatVe = x.NgayDatVe,
                MaKh = x.MaKh,
                TenKhachHang = khachHang.TenKh,
            }).ToList();
            var chiTietVes = filterChiTietVes.Join(ves, x => x.MaVe, ve => ve.MaVe, (x, ve) => new
            {
                MaVe = x.MaVe,
                NgayDatVe = ve.NgayDatVe,
                MaKh = ve.MaKh,
                TenKhachHang = ve.TenKhachHang,
                MaChuyenBay = x.MaChuyenBay,
                LoaiVe = x.LoaiVe,
                SoLuong = x.SoLuong,
                TongGia = x.TongGia,
            }).ToList();

            return Ok(chiTietVes);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(string inputMaVe)
        {
            var chiTietVe = await _context.Chitietves.FindAsync(inputMaVe);
            if (chiTietVe == null)
            {
                return NotFound(chiTietVe);
            }
            _context.Chitietves.Remove(chiTietVe);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult> UpdateAsync(Chitietve input)
        {
            if (input == null)
            {
                return StatusCode(400);
            }
            _context.Chitietves.Update(input);
            await _context.SaveChangesAsync();
            return Ok(input);
        }
    }
}
