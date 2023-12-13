using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingTourWeb_WebAPI.ViewModels;
using BookingTourWeb_WebAPI.Models;
namespace BookingTourWeb_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongTinVesController : ControllerBase
    {
        private readonly DvmayBayContext _context;

        public ThongTinVesController(DvmayBayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThongTinVe>>> GetThongTinVe()
        {
            //return await _context.ThongTinKhachHang.ToListAsync();
            var ThongTinVe = (from ve in _context.Ves
                                     join chitietve in _context.Chitietves
                                     on ve.MaVe equals chitietve.MaVe
                                     join khachhang in _context.Khachhangs
                                     on ve.MaKh equals khachhang.MaKh
                                     select new ThongTinVe()
                                     {
                                         MaVe = ve.MaVe,
                                         MaKH = ve.MaKh,
                                         HoTenKH=khachhang.TenKh,
                                         NgayDatVe = ve.NgayDatVe,
                                         LoaiVe = chitietve.LoaiVe,
                                         MaChuyenBay = chitietve.MaChuyenBay,
                                         Soluong = chitietve.SoLuong,
                                         TinhTrang = chitietve.TinhTrang,
                                         TongGia = chitietve.TongGia
                                     })
                                         
            .ToListAsync();
            //return Ok(ThongTinKhachHang);
            return await ThongTinVe;
        }
    }
}
