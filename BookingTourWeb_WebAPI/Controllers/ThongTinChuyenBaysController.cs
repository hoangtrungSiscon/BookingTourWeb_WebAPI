using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingTourWeb_WebAPI.Models;

namespace BookingTourWeb_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongTinChuyenBaysController : ControllerBase
    {
        private readonly DvmayBayContext _context;

        public ThongTinChuyenBaysController(DvmayBayContext context)
        {
            _context = context;
        }

        // GET: api/ThongTinChuyenBays
        //[HttpGet]
        //public Task<ActionResult<IEnumerable<ThongTinChuyenBay>>> GetThongTinChuyenBay()
        //{
        [HttpGet]
        public IEnumerable<ThongTinChuyenBay> GetQuanLyChuyenBay()
        {
            //return await _context.ThongTinChuyenBay.ToListAsync();
            var thongtinchuyenbay = (from chuyenbay in _context.Chuyenbays
                                     join maybay in _context.Maybays
                                     on chuyenbay.MaMayBay equals maybay.MaMayBay
                                     select new ThongTinChuyenBay()
                                     {
                                         MaChuyenBay = chuyenbay.MaChuyenBay,
                                         MaMayBay = chuyenbay.MaMayBay,
                                         TenMayBay = maybay.TenMayBay,
                                         NoiXuatPhat = chuyenbay.NoiXuatPhat,
                                         NoiDen = chuyenbay.NoiDen,
                                         NgayXuatPhat = chuyenbay.NgayXuatPhat,
                                         GioBay = chuyenbay.GioBay,
                                         SoLuongVeBsn = chuyenbay.SoLuongVeBsn,
                                         SoLuongVeEco = chuyenbay.SoLuongVeEco,
                                         DonGia = chuyenbay.DonGia,
                                     })
            .ToList();
            return thongtinchuyenbay;
        }

        // GET: api/ThongTinChuyenBays/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<ThongTinChuyenBay>> GetThongTinChuyenBay(string id)
        //{
        //  if (_context.ThongTinChuyenBay == null)
        //  {
        //      return NotFound();
        //  }
        //    var thongTinChuyenBay = await _context.ThongTinChuyenBay.FindAsync(id);

        //    if (thongTinChuyenBay == null)
        //    {
        //        return NotFound();
        //    }

        //    return thongTinChuyenBay;
        //}

        //// PUT: api/ThongTinChuyenBays/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutThongTinChuyenBay(string id, ThongTinChuyenBay thongTinChuyenBay)
        //{
        //    if (id != thongTinChuyenBay.MaChuyenBay)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(thongTinChuyenBay).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ThongTinChuyenBayExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/ThongTinChuyenBays
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<ThongTinChuyenBay>> PostThongTinChuyenBay(ThongTinChuyenBay thongTinChuyenBay)
        //{
        //  if (_context.ThongTinChuyenBay == null)
        //  {
        //      return Problem("Entity set 'DvmayBayContext.ThongTinChuyenBay'  is null.");
        //  }
        //    _context.ThongTinChuyenBay.Add(thongTinChuyenBay);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (ThongTinChuyenBayExists(thongTinChuyenBay.MaChuyenBay))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetThongTinChuyenBay", new { id = thongTinChuyenBay.MaChuyenBay }, thongTinChuyenBay);
        //}

        //// DELETE: api/ThongTinChuyenBays/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteThongTinChuyenBay(string id)
        //{
        //    if (_context.ThongTinChuyenBay == null)
        //    {
        //        return NotFound();
        //    }
        //    var thongTinChuyenBay = await _context.ThongTinChuyenBay.FindAsync(id);
        //    if (thongTinChuyenBay == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.ThongTinChuyenBay.Remove(thongTinChuyenBay);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool ThongTinChuyenBayExists(string id)
        //{
        //    return (_context.ThongTinChuyenBay?.Any(e => e.MaChuyenBay == id)).GetValueOrDefault();
        //}
    }
}
