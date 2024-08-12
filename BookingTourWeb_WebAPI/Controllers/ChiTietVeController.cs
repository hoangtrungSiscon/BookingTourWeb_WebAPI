using BookingTourWeb_WebAPI.Models;
using BookingTourWeb_WebAPI.Models.InputModels;
using BookingTourWeb_WebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;

namespace BookingTourWeb_WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChiTietVeController : ControllerBase
    {
        private readonly DVMayBayContext _context;

        public ChiTietVeController(DVMayBayContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<object>>> GetAllAsync()
        {
            var ves = _context.Ves.ToList().Join(_context.Khachhangs, x => x.MaKh, khachHang => khachHang.MaKh, (x, khachHang) => new
            {
                MaVe = x.MaVe,
                NgayDatVe = x.NgayDatVe,
                MaKh = x.MaKh,
                TenKhachHang = khachHang.TenKh,
            }).ToList();
            var hoadons = _context.Hoadons.ToList().Join(_context.Phuongthucthanhtoans, x => x.KieuThanhToan, pttt => pttt.KieuThanhToan, (x, pttt) => new {
                MaGiaoDich = x.MaGiaoDich,
                TenKieuThanhToan = pttt.TenKieuThanhToan,
                MaVe = x.MaVe,
            }).ToList();
            var hoadonves = ves.Join(hoadons, ve => ve.MaVe, hoadon => hoadon.MaVe, (ve, hoadon) => new {
                MaVe = ve.MaVe,
                NgayDatVe = ve.NgayDatVe,
                MaKh = ve.MaKh,
                TenKhachHang = ve.TenKhachHang,
                MaGiaoDich = hoadon.MaGiaoDich,
                TenKieuThanhToan = hoadon.TenKieuThanhToan
            }).ToList();
            var chiTietVes = _context.Chitietves.ToList().Join(hoadonves, x => x.MaVe, ve => ve.MaVe, (x, ve) => new
            {
                MaCTV = x.MaCtv,
                MaVe = x.MaVe,
                NgayDatVe = ve.NgayDatVe,
                MaKh = ve.MaKh,
                TenKhachHang = ve.TenKhachHang,
                MaChuyenBay = x.MaChuyenBay,
                LoaiVe = x.LoaiVe,
                SoLuong = x.SoLuong,
                TinhTrang = x.TinhTrang,
                TongGia = x.TongGia,
                MaGiaoDich = ve.MaGiaoDich,
                TenKieuThanhToan = ve.TenKieuThanhToan,
            }).ToList();
            return Ok(chiTietVes);
        }

        [HttpPost]
        public async Task<ActionResult<List<object>>> FilterChiTietVeAsync(InputFilterGuestsChuyenBay input)
        {
            var filterVe = await _context.Ves.Where(x => x.NgayDatVe >= DateTime.Parse(input.bookDate)).ToListAsync();
            var filterChiTietVes = await _context.Chitietves.Where(x => filterVe.Select(x => x.MaVe).Contains(x.MaVe)).ToListAsync();
            var chuyenBays = await _context.Chuyenbays.Where(x => x.NgayXuatPhat == DateTime.Parse(input.startDate) && x.NoiXuatPhat == input.fromPlace && x.NoiDen == input.toPlace).ToListAsync();
            var ves = filterVe.Join(_context.Khachhangs, x => x.MaKh, khachHang => khachHang.MaKh, (x, khachHang) => new
            {
                MaVe = x.MaVe,
                NgayDatVe = x.NgayDatVe,
                MaKh = x.MaKh,
                TenKhachHang = khachHang.TenKh,
            }).ToList();
            var hoadons = _context.Hoadons.ToList().Join(_context.Phuongthucthanhtoans, x => x.KieuThanhToan, pttt => pttt.KieuThanhToan, (x, pttt) => new {
                MaGiaoDich = x.MaGiaoDich,
                TenKieuThanhToan = pttt.TenKieuThanhToan,
                MaVe = x.MaVe,
            }).ToList();
            var hoadonves = ves.Join(hoadons, ve => ve.MaVe, hoadon => hoadon.MaVe, (ve, hoadon) => new {
                MaVe = ve.MaVe,
                NgayDatVe = ve.NgayDatVe,
                MaKh = ve.MaKh,
                TenKhachHang = ve.TenKhachHang,
                MaGiaoDich = hoadon.MaGiaoDich,
                TenKieuThanhToan = hoadon.TenKieuThanhToan
            }).ToList();
            var chiTietVes = filterChiTietVes.Join(hoadonves, x => x.MaVe, ve => ve.MaVe, (x, ve) => new
            {
                MaCTV = x.MaCtv,
                MaVe = x.MaVe,
                NgayDatVe = ve.NgayDatVe,
                MaKh = ve.MaKh,
                TenKhachHang = ve.TenKhachHang,
                MaChuyenBay = x.MaChuyenBay,
                LoaiVe = x.LoaiVe,
                SoLuong = x.SoLuong,
                TinhTrang = x.TinhTrang,
                TongGia = x.TongGia,
            }).Where(x => chuyenBays.Select(xx => xx.MaChuyenBay).Contains(x.MaChuyenBay) && x.LoaiVe == input.typeSeat).ToList();

            return Ok(chiTietVes);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(long inputMaVe)
        {
            var MachiTietVe = await _context.Chitietves.Where(x => x.MaVe == inputMaVe).FirstOrDefaultAsync();
            if (MachiTietVe == null)
            {
                return NotFound(MachiTietVe);
            }
            _context.Chitietves.Remove(MachiTietVe);
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

            if (input.TinhTrang == "Đã duyệt")
            {
                var hoadon = _context.Hoadons.Where(f => f.MaVe == input.MaVe).FirstOrDefault();

                if (hoadon == null)
                {
                    return BadRequest();
                }

                hoadon.KieuThanhToan = "CASH";

                hoadon.TinhTrangThanhToan = "Ðã thanh toán";

                hoadon.NgayThanhToan = DateTime.Now;

                _context.SaveChanges();
            }


            var chuyenBay = await _context.Chuyenbays.Where(x => x.MaChuyenBay == input.MaChuyenBay).FirstOrDefaultAsync();
            if (input.LoaiVe == "BSN")
            {
                chuyenBay.SoLuongVeBsn = await _context.Chitietves.Where(f => f.MaChuyenBay == chuyenBay.MaChuyenBay && f.LoaiVe == "BSN" && f.TinhTrang != "Đã hủy").SumAsync(b => b.SoLuong);
            }
            else
            {
                chuyenBay.SoLuongVeEco = await _context.Chitietves.Where(f => f.MaChuyenBay == chuyenBay.MaChuyenBay && f.LoaiVe == "ECO" && f.TinhTrang != "Đã hủy").SumAsync(b => b.SoLuong);
            }
            _context.Chuyenbays.Update(chuyenBay);
            _context.SaveChanges();


            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(InputChiTietVe request)
        {
            var chuyenBay = await _context.Chuyenbays.Where(x => x.MaChuyenBay == request.MaChuyenBay).FirstOrDefaultAsync();
            if (request.LoaiVe == "BSN")
            {

                var bsnSeatsAvailable = _context.Maybays.Where(e => e.MaMayBay == chuyenBay.MaMayBay).Sum(a => a.SlgheBsn) - _context.Chitietves.Where(c => c.MaChuyenBay == chuyenBay.MaChuyenBay && c.LoaiVe == "BSN" && c.TinhTrang != "Đã hủy").Sum(b => b.SoLuong);

                if (request.SoLuong > bsnSeatsAvailable)
                {
                    return BadRequest();
                }
            }

            if (request.LoaiVe == "ECO")
            {


                var ecoSeatsAvailable = _context.Maybays.Where(e => e.MaMayBay == chuyenBay.MaMayBay).Sum(a => a.SlgheEco) - _context.Chitietves.Where(c => c.MaChuyenBay == chuyenBay.MaChuyenBay && c.LoaiVe == "ECO" && c.TinhTrang != "Đã hủy").Sum(b => b.SoLuong);

                if (request.SoLuong > ecoSeatsAvailable)
                {
                    return BadRequest();
                }
            }

            var kh = await _context.Khachhangs.Where(x => x.GmailKh == request.GmailKh).FirstOrDefaultAsync();
            var newVe = new Ve() { MaVe = request.MaVe, MaKh = kh.MaKh, MaKhNavigation = kh, NgayDatVe = DateTime.Parse(request.NgayDatVe) };
            await _context.Ves.AddAsync(newVe);
            _context.SaveChanges();
            var newCTV = new Chitietve() { MaCtv = 0, MaVe = newVe.MaVe, LoaiVe = request.LoaiVe, MaChuyenBay = request.MaChuyenBay, SoLuong = request.SoLuong, TinhTrang = "Đang xác nhận", TongGia = request.TongGia };
            _context.Chitietves.Add(newCTV);
            _context.SaveChanges();

            if (request.LoaiVe == "BSN")
            {
                chuyenBay.SoLuongVeBsn = await _context.Chitietves.Where(f => f.MaChuyenBay == chuyenBay.MaChuyenBay && f.LoaiVe == "BSN" && f.TinhTrang != "Đã hủy").SumAsync(b => b.SoLuong);
            }
            else
            {
                chuyenBay.SoLuongVeEco = await _context.Chitietves.Where(f => f.MaChuyenBay == chuyenBay.MaChuyenBay && f.LoaiVe == "ECO" && f.TinhTrang != "Đã hủy").SumAsync(b => b.SoLuong);
            }
            _context.Chuyenbays.Update(chuyenBay);
            _context.SaveChanges();

            var invoice = new Hoadon()
            {
                MaVe = newVe.MaVe,
                TinhTrangThanhToan = "Chưa thanh toán"
            };

            _context.Hoadons.Add(invoice);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThongTinChuyenBay>>> GetChiTietVe(long? MaCTV)
        {
            var query = _context.Chitietves
                //.Include(f => f.MaChuyenBayNavigation)
                .AsQueryable();
            query = query.Where(f => f.MaCtv == MaCTV);

            var chitietve = await query.Select(f => new
            {

            }).FirstOrDefaultAsync();
            if (chitietve == null)
            {
                return NotFound("ChiTietVe not found.");
            }
            //return Ok(thongtinchuyenbay);
            return Ok(chitietve);
        }

        [HttpPost]
        public async Task<IActionResult> CheckValidity(InputChiTietVe request)
        {
            if ((request.LoaiVe != "BSN" && request.LoaiVe != "ECO") || request.SoLuong <= 0 || !_context.Chuyenbays.Any(f => f.MaChuyenBay == request.MaChuyenBay)) 
                return BadRequest();

            var chuyenBay = await _context.Chuyenbays.Where(x => x.MaChuyenBay == request.MaChuyenBay).FirstOrDefaultAsync();
            if (request.LoaiVe == "BSN")
            {

                var bsnSeatsAvailable = _context.Maybays.Where(e => e.MaMayBay == chuyenBay.MaMayBay).Sum(a => a.SlgheBsn) - _context.Chitietves.Where(c => c.MaChuyenBay == chuyenBay.MaChuyenBay && c.LoaiVe == "BSN" && c.TinhTrang != "Đã hủy").Sum(b => b.SoLuong);

                if (request.SoLuong > bsnSeatsAvailable)
                {
                    return BadRequest();
                }
            }

            if (request.LoaiVe == "ECO")
            {


                var ecoSeatsAvailable = _context.Maybays.Where(e => e.MaMayBay == chuyenBay.MaMayBay).Sum(a => a.SlgheEco) - _context.Chitietves.Where(c => c.MaChuyenBay == chuyenBay.MaChuyenBay && c.LoaiVe == "ECO" && c.TinhTrang != "Đã hủy").Sum(b => b.SoLuong);

                if (request.SoLuong > ecoSeatsAvailable)
                {
                    return BadRequest() ;
                }
            }
            return Ok();
        }
    }
}