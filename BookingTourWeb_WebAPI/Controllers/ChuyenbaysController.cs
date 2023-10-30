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
    public class ChuyenbaysController : ControllerBase
    {
        private readonly DvmayBayContext _context;

        public ChuyenbaysController(DvmayBayContext context)
        {
            _context = context;
        }

        // GET: api/Chuyenbays
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chuyenbay>>> GetChuyenbays()
        {
            if (_context.Chuyenbays == null)
            {
                return NotFound();
            }
            return await _context.Chuyenbays.ToListAsync();
        }
        //[HttpGet]
        //public async Task<ActionResult<List<Chuyenbay>>> GetChuyenbays()
        //{
        //    if (_context.Chuyenbays == null)
        //    {
        //        return NotFound();
        //    }
        //    return await _context.Chuyenbays.ToListAsync();
        //}

        // GET: api/Chuyenbays/5





        //[HttpGet]
        //public IEnumerable<ThongTinChuyenBay> GetQuanLyChuyenBay()
        //{




        //return await _context.Chuyenbays.ToListAsync();
        //var result = _context.Chuyenbays
        //    .Join(_context.Maybays,
        //    chuyenbay => chuyenbay.MaMayBay,
        //    maybay => maybay.MaMayBay,
        //    (chuyenbay, maybay) => new
        //    {
        //        ChuyenbayId = chuyenbay.MaChuyenBay,
        //        ChuyenbayInfo = chuyenbay,
        //        MaybayInfo = maybay
        //    })
        //.ToList();

        //return Ok(result);
        //-------------------------------------------------------------------------
        //var thongtinchuyenbay = (from chuyenbay in _context.Chuyenbays
        //                         join maybay in _context.Maybays
        //                         on chuyenbay.MaMayBay equals maybay.MaMayBay
        //                         select new
        //                         {
        //                             Machuyen = chuyenbay.MaChuyenBay,
        //                             Mamaybay = chuyenbay.MaMayBay,
        //                             Tenmaybay = maybay.TenMayBay,
        //                             Noidi = chuyenbay.NoiXuatPhat,
        //                             Noiden = chuyenbay.NoiDen,
        //                             Ngayxuatphat = chuyenbay.NgayXuatPhat,
        //                             Giobay = chuyenbay.GioBay,
        //                             Sl_BSN = chuyenbay.SoLuongVeBsn,
        //                             SL_ECO = chuyenbay.SoLuongVeEco,
        //                             Gia = chuyenbay.DonGia,
        //                         })
        //.ToListAsync();
        //return Ok(thongtinchuyenbay);

        //-------------------------------------------------------------------------------------
        //var thongtinchuyenbay = (
        //    _context.Chuyenbays
        //    .Join(_context.Maybays,
        //        p => p.MaMayBay,
        //        e => e.MaMayBay,
        //        (p, e) => new
        //        {
        //            Machuyen = p.MaChuyenBay,
        //            Mamaybay = p.MaMayBay,
        //            Tenmaybay = e.TenMayBay,
        //            Noidi = p.NoiXuatPhat,
        //            Noiden = p.NoiDen,
        //            Ngayxuatphat = p.NgayXuatPhat,
        //            Giobay = p.GioBay,
        //            Sl_BSN = p.SoLuongVeBsn,
        //            SL_ECO = p.SoLuongVeEco,
        //            Gia = p.DonGia,
        //        }
        //    ).ToList()
        //    );
        //return Ok(thongtinchuyenbay);
        //---------------------------------------------------------------

        //    var thongtinchuyenbay = (from chuyenbay in _context.Chuyenbays
        //                             join maybay in _context.Maybays
        //                             on chuyenbay.MaMayBay equals maybay.MaMayBay
        //                             select new ThongTinChuyenBay()
        //                             {
        //                                 MaChuyenBay = chuyenbay.MaChuyenBay,
        //                                 MaMayBay = chuyenbay.MaMayBay,
        //                                 TenMayBay = maybay.TenMayBay,
        //                                 NoiXuatPhat = chuyenbay.NoiXuatPhat,
        //                                 NoiDen = chuyenbay.NoiDen,
        //                                 NgayXuatPhat = chuyenbay.NgayXuatPhat,
        //                                 GioBay = chuyenbay.GioBay,
        //                                 SoLuongVeBsn = chuyenbay.SoLuongVeBsn,
        //                                 SoLuongVeEco = chuyenbay.SoLuongVeEco,
        //                                 DonGia = chuyenbay.DonGia,
        //                             })
        //    .ToList();
        //    return thongtinchuyenbay;
        //}



        [HttpGet("{id}")]
        public async Task<ActionResult<Chuyenbay>> GetChuyenbay(string id)
        {
          if (_context.Chuyenbays == null)
          {
              return NotFound();
          }
            var chuyenbay = await _context.Chuyenbays.FindAsync(id);

            if (chuyenbay == null)
            {
                return NotFound();
            }

            return chuyenbay;
        }

        // PUT: api/Chuyenbays/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChuyenbay(string id, Chuyenbay chuyenbay)
        {
            if (id != chuyenbay.MaChuyenBay)
            {
                return BadRequest();
            }

            _context.Entry(chuyenbay).State = EntityState.Modified;

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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Chuyenbay>> PostChuyenbay(Chuyenbay chuyenbay)
        {
          if (_context.Chuyenbays == null)
          {
              return Problem("Entity set 'DvmayBayContext.Chuyenbays'  is null.");
          }
            _context.Chuyenbays.Add(chuyenbay);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ChuyenbayExists(chuyenbay.MaChuyenBay))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetChuyenbay", new { id = chuyenbay.MaChuyenBay }, chuyenbay);
        }

        // DELETE: api/Chuyenbays/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChuyenbay(string id)
        {
            if (_context.Chuyenbays == null)
            {
                return NotFound();
            }
            var chuyenbay = await _context.Chuyenbays.FindAsync(id);
            if (chuyenbay == null)
            {
                return NotFound();
            }

            _context.Chuyenbays.Remove(chuyenbay);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChuyenbayExists(string id)
        {
            return (_context.Chuyenbays?.Any(e => e.MaChuyenBay == id)).GetValueOrDefault();
        }
    }
}
