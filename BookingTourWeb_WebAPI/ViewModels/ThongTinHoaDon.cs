using System.ComponentModel.DataAnnotations;

namespace BookingTourWeb_WebAPI.ViewModels
{
    public class ThongTinHoaDon
    {
        [Key]
        public int Idhoadon { get; set; }

        public long? MaVe { get; set; }

        public string? MaChuyenBay { get; set; }

        public string? KieuThanhToan { get; set; }

        public string? MaGiaoDich { get; set; }

        public string? TinhTrangThanhToan { get; set; }

        public DateTime? NgayDatVe { get; set; }

        public decimal TongGia { get; set; }

        public DateTime? NgayThanhToan { get; set; }

        public string TenKh { get; set; } = null!;

        public string Sdt { get; set; } = null!;

        public string GmailKh { get; set; } = null!;

        public string Phai { get; set; } = null!;

        public string LoaiGhe { get; set; } = null;

        public int SoGhe { get; set; } = 0;
    }
}
