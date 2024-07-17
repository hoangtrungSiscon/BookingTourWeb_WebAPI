using System;
using System.Collections.Generic;

namespace BookingTourWeb_WebAPI.Models;

public partial class Phuongthucthanhtoan
{
    public string KieuThanhToan { get; set; } = null!;

    public string TenKieuThanhToan { get; set; } = null!;

    public virtual ICollection<Hoadon> Hoadons { get; set; } = new List<Hoadon>();
}
