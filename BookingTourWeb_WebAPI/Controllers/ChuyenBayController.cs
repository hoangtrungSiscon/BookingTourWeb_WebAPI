using BookingTourWeb_WebAPI.Models;
using BookingTourWeb_WebAPI.Models.InputModels;
using BookingTourWeb_WebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BookingTourWeb_WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChuyenBayController : ControllerBase
    {
        private readonly DvmayBayContext _context;

        public ChuyenBayController(DvmayBayContext context)
        {
            this._context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<ThongTinChuyenBay>>> GetAllAsync()
        {
            var flightList = await _context.Chuyenbays.Include(f => f.MaMayBayNavigation).Select(f => new ThongTinChuyenBay
            {
                MaChuyenBay = f.MaChuyenBay,
                MaMayBay = f.MaMayBay,
                TenMayBay = f.MaMayBayNavigation.TenMayBay,
                NoiXuatPhat = f.NoiXuatPhat,
                NoiDen = f.NoiDen,
                NgayXuatPhat = f.NgayXuatPhat,
                GioBay = f.GioBay,
                DonGia = f.DonGia,
                SoLuongVeBsn = _context.Maybays.Where(e => e.MaMayBay == f.MaMayBay).Sum(a => a.SlgheBsn) - _context.Chitietves.Where(c => c.MaChuyenBay == f.MaChuyenBay && c.LoaiVe == "BSN" && c.TinhTrang != "Đã hủy").Sum(b => b.SoLuong),
                SoLuongVeEco = _context.Maybays.Where(e => e.MaMayBay == f.MaMayBay).Sum(a => a.SlgheEco) - _context.Chitietves.Where(c => c.MaChuyenBay == f.MaChuyenBay && c.LoaiVe == "ECO" && c.TinhTrang != "Đã hủy").Sum(b => b.SoLuong),
            }).ToListAsync();
            return flightList;
        }

        [HttpPost]
        public async Task<ActionResult<List<Chuyenbay>>> FilterChuyenBayAsync(InputFilterChuyenBay input)
        {
            var data = _context.Chuyenbays.AsQueryable();
            if (input == null)
            {
                return await _context.Chuyenbays.ToListAsync();
            }
            if (!string.IsNullOrEmpty(input.fromPlace))
            {
                data = data.Where(f => f.NoiXuatPhat == input.fromPlace);
            }
            if (!string.IsNullOrEmpty(input.toPlace))
            {
                data = data.Where(f => f.NoiDen == input.toPlace);
            }
            if (!string.IsNullOrEmpty(input.startDate))
            {
                data = data.Where(f => f.NgayXuatPhat >= (DateTime.Parse(input.startDate)));
            }
            foreach (var flight in data)
            {
                flight.SoLuongVeBsn = _context.Maybays.Where(e => e.MaMayBay == flight.MaMayBay).Sum(f => f.SlgheBsn) - _context.Chitietves.Where(c => c.MaChuyenBay == flight.MaChuyenBay && c.LoaiVe == "BSN" && c.TinhTrang != "Đã hủy").Sum(b => b.SoLuong);
                flight.SoLuongVeEco = _context.Maybays.Where(e => e.MaMayBay == flight.MaMayBay).Sum(f => f.SlgheEco) - _context.Chitietves.Where(c => c.MaChuyenBay == flight.MaChuyenBay && c.LoaiVe == "ECO" && c.TinhTrang != "Đã hủy").Sum(b => b.SoLuong);
            }
            return Ok(await data.ToListAsync());
        }
        [HttpGet]
        public async Task<ActionResult<object>> GetByCodeAsync(string code)
        {
            //var chuyenBay = await _context.Chuyenbays.Where(x => x.MaChuyenBay == code).Join(_context.Maybays, x => x.MaMayBay, mayBay => mayBay.MaMayBay, (x, mayBay) => new
            //{
            //    chuyenBay = x,
            //    mayBay = mayBay
            //}).FirstOrDefaultAsync();
            //if (chuyenBay != null)
            //{
            //    return Ok(chuyenBay);
            //}
            //return NotFound();
            var query = _context.Chuyenbays.Include(f => f.MaMayBayNavigation).AsQueryable();

            query = query.Where(f => f.MaChuyenBay == code);

            var thongtinchuyenbay = await query.Select(f => new
            {
                mayBay = f.MaMayBayNavigation,
                chuyenBay = new Chuyenbay
                {
                    MaChuyenBay = f.MaChuyenBay,
                    MaMayBay = f.MaMayBay,
                    NoiXuatPhat = f.NoiXuatPhat,
                    NoiDen = f.NoiDen,
                    NgayXuatPhat = f.NgayXuatPhat,
                    SoLuongVeBsn = _context.Maybays.Where(e => e.MaMayBay == f.MaMayBay).Sum(a => a.SlgheBsn) - _context.Chitietves.Where(c => c.MaChuyenBay == f.MaChuyenBay && c.LoaiVe == "BSN" && c.TinhTrang != "Đã hủy").Sum(b => b.SoLuong),
                    SoLuongVeEco = _context.Maybays.Where(e => e.MaMayBay == f.MaMayBay).Sum(a => a.SlgheEco) - _context.Chitietves.Where(c => c.MaChuyenBay == f.MaChuyenBay && c.LoaiVe == "ECO" && c.TinhTrang != "Đã hủy").Sum(b => b.SoLuong),
                    GioBay = f.GioBay,
                    DonGia = f.DonGia
                }
            }).FirstOrDefaultAsync();
            //foreach (var item in thongtinchuyenbay)
            //{
            //    item.SoLuongVeBsn = await _context.Chitietves
            //        .Where(ctv => ctv.MaChuyenBay == item.MaChuyenBay && ctv.LoaiVe == "BSN")
            //        .SumAsync(ctv => ctv.SoLuong);

            //    item.SoLuongVeEco = await _context.Chitietves
            //        .Where(ctv => ctv.MaChuyenBay == item.MaChuyenBay && ctv.LoaiVe == "ECO")
            //        .SumAsync(ctv => ctv.SoLuong);
            //}
            if (thongtinchuyenbay == null)
            {
                return NotFound("ThongTinChuyenBay not found.");
            }
            //return Ok(thongtinchuyenbay);
            return thongtinchuyenbay;
        }

    }
}
