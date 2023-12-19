using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTourWeb_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketHistoryController : ControllerBase
    {
        private readonly DvmayBayContext _context;

        public TicketHistoryController(DvmayBayContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<TicketHistory>>> GetHistoryTicketList (int id)
        {
            var query = _context.Chitietves.Include(f => f.MaVeNavigation).Include(f => f.MaChuyenBayNavigation).AsQueryable();
            query = query.Where(c => c.MaVeNavigation.MaKh == id);
            var data = await query.Select(f => new TicketHistory
            {
                idKH = f.MaVeNavigation.MaKh,
                idTicket = f.MaVeNavigation.MaVe,
                ngaydatve = f.MaVeNavigation.NgayDatVe,
                noidi = f.MaChuyenBayNavigation.NoiXuatPhat,
                noiden = f.MaChuyenBayNavigation.NoiDen,
                loai = f.LoaiVe,
                soluong = f.SoLuong,
                ngaydi = f.MaChuyenBayNavigation.NgayXuatPhat,
                machuyen = f.MaChuyenBay,
                gia = f.MaChuyenBayNavigation.DonGia,
                tinhtrang = f.TinhTrang,
            }).ToListAsync();
            if(data != null)
            {
                NoContent();
            }
            return Ok(data);
        }
    }
}
