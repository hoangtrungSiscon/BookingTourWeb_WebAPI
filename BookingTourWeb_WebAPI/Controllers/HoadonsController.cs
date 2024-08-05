using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingTourWeb_WebAPI.Models;
using NuGet.Versioning;

namespace BookingTourWeb_WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HoadonsController : ControllerBase
    {
        private readonly DVMayBayContext _context;

        public HoadonsController(DVMayBayContext context)
        {
            _context = context;
        }

        // GET: api/Hoadons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hoadon>>> GetAllInvoices()
        {
            if (_context.Hoadons == null)
            {
                return NotFound();
            }
            var query = _context.Hoadons.AsQueryable();

            return await _context.Hoadons.Include(f => f.MaVeNavigation).ToListAsync();
        }

        [HttpGet("{machuyenbay}")]
        public async Task<ActionResult<Object>> GetFlightInfoOfInvoice(string machuyenbay)
        {
            if (_context.Chuyenbays == null)
            {
                return NotFound();
            }
            var query = await _context.Chuyenbays.Include(f => f.MaMayBayNavigation).Where(f => f.MaChuyenBay == machuyenbay).AsQueryable().FirstOrDefaultAsync();
            
            dynamic data = new System.Dynamic.ExpandoObject();
            data.MaChuyenBay = query.MaChuyenBay;
            data.TenMayBay = query.MaMayBayNavigation.TenMayBay;
            data.NoiXuatPhat = query.NoiXuatPhat;
            data.NoiDen = query.NoiDen;
            data.NgayXuatPhat = query.NgayXuatPhat;
            data.GioBay = query.GioBay;

            return data;
        }

        [HttpGet("{id}/{accountId}")]
        public async Task<ActionResult<ThongTinHoaDon>> GetInvoiceById(int id, int accountId)
        {
            if (_context.Hoadons == null)
            {
                return NotFound();
            }
            var hoadon = await (from hd in _context.Hoadons
                                join ve in _context.Ves on hd.MaVe equals ve.MaVe
                                join kh in _context.Khachhangs on hd.MaVeNavigation.MaKh equals kh.MaKh
                                join ctv in _context.Chitietves on hd.MaVe equals ctv.MaVe
                                where hd.Idhoadon == id && kh.MaTaiKhoan == accountId
                                select new ThongTinHoaDon
                                {
                                    Idhoadon = hd.Idhoadon,
                                    MaVe = hd.MaVe,
                                    KieuThanhToan = hd.KieuThanhToanNavigation.TenKieuThanhToan,
                                    MaGiaoDich = hd.MaGiaoDich,
                                    TinhTrangThanhToan = hd.TinhTrangThanhToan,
                                    NgayThanhToan = hd.NgayThanhToan,
                                    TenKh = kh.TenKh,
                                    Sdt = kh.Sdt,
                                    GmailKh = kh.GmailKh,
                                    Phai = kh.Phai,
                                    NgayDatVe = ve.NgayDatVe,
                                    TongGia = ctv.TongGia,
                                    MaChuyenBay = ctv.MaChuyenBay,
                                    LoaiGhe = ctv.LoaiVe,
                                    SoGhe = ctv.SoLuong,
                                }).FirstOrDefaultAsync();

            if (hoadon == null)
            {
                return NotFound();
            }

            return hoadon;
        }

        // GET: api/Hoadons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ThongTinHoaDon>>> GetInvoicesByAccountId(int id)
        {
            if (_context.Hoadons == null)
            {
                return NotFound();
            }

            var hoadon = await (from hd in _context.Hoadons
                                join ve in _context.Ves on hd.MaVe equals ve.MaVe
                                join kh in _context.Khachhangs on hd.MaVeNavigation.MaKh equals kh.MaKh
                                join ctv in _context.Chitietves on hd.MaVe equals ctv.MaVe
                                where hd.MaVeNavigation.MaKhNavigation.MaTaiKhoan == id
                                select new ThongTinHoaDon
                                {
                                    Idhoadon = hd.Idhoadon,
                                    MaVe = hd.MaVe,
                                    KieuThanhToan = hd.KieuThanhToanNavigation.TenKieuThanhToan,
                                    MaGiaoDich = hd.MaGiaoDich,
                                    TinhTrangThanhToan = hd.TinhTrangThanhToan,
                                    NgayThanhToan = hd.NgayThanhToan,
                                    TenKh = kh.TenKh,
                                    Sdt = kh.Sdt,
                                    GmailKh = kh.GmailKh,
                                    Phai = kh.Phai,
                                    NgayDatVe = ve.NgayDatVe,
                                    TongGia = ctv.TongGia,
                                    MaChuyenBay = ctv.MaChuyenBay,
                                    LoaiGhe = ctv.LoaiVe,
                                    SoGhe = ctv.SoLuong,
                                }).ToListAsync();

            if (hoadon == null)
            {
                return NotFound();
            }

            return hoadon;
        }

        // PUT: api/Hoadons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/{accountId}/{kieuthanhtoan}/{magiaodich}")]
        public async Task<IActionResult> UpdatePayStatus(int id, int accountId, string kieuthanhtoan, string magiaodich)
        {
            if (!_context.Hoadons.Any(f => f.Idhoadon == id))
            {
                return BadRequest();
            }

            var hoadon = await _context.Hoadons.FirstOrDefaultAsync(f => f.Idhoadon == id && f.MaVeNavigation.MaKh == accountId);

            _context.Entry(hoadon).State = EntityState.Modified;

            if (hoadon == null)
            {
                return NotFound("Invoice not found");
            }

            if (hoadon.TinhTrangThanhToan == "Đã thanh toán")
            {
                return NoContent();
            }

            hoadon.TinhTrangThanhToan = "Đã thanh toán";
            hoadon.MaGiaoDich = magiaodich;
            hoadon.KieuThanhToan = kieuthanhtoan;
            hoadon.NgayThanhToan = DateTime.Now;

            _context.Hoadons.Update(hoadon);

            var ctv = _context.Chitietves.FirstOrDefault(f => f.MaVe == hoadon.MaVe);

            _context.Entry(ctv).State = EntityState.Modified;

            if (ctv == null)
            {
                return NotFound("Invoice not found");
            }

            ctv.TinhTrang = "Đã duyệt";

            _context.Chitietves.Update(ctv);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Hoadons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hoadon>> CreateInvoice(Hoadon hoadon)
        {
            if (_context.Hoadons == null)
            {
                return Problem("Entity set 'DvmayBayContext.Hoadons'  is null.");
            }
            _context.Hoadons.Add(hoadon);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHoadon", new { id = hoadon.Idhoadon }, hoadon);
        }

        // DELETE: api/Hoadons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHoadon(int id)
        {
            if (_context.Hoadons == null)
            {
                return NotFound();
            }
            var hoadon = await _context.Hoadons.FindAsync(id);
            if (hoadon == null)
            {
                return NotFound();
            }

            _context.Hoadons.Remove(hoadon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HoadonExists(int id)
        {
            return (_context.Hoadons?.Any(e => e.Idhoadon == id)).GetValueOrDefault();
        }
    }
}
