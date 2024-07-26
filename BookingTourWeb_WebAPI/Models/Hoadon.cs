using System;
using System.Collections.Generic;

namespace BookingTourWeb_WebAPI.Models
{
    public partial class Hoadon
    {
        public int Idhoadon { get; set; }
        public long? MaVe { get; set; }
        public string? KieuThanhToan { get; set; }
        public string? MaGiaoDich { get; set; }
        public string? TinhTrangThanhToan { get; set; }
        public DateTime? NgayThanhToan { get; set; }

        public virtual Phuongthucthanhtoan? KieuThanhToanNavigation { get; set; }
        public virtual Ve? MaVeNavigation { get; set; }
    }
}
