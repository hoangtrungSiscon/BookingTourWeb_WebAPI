using System;
using System.Collections.Generic;

namespace BookingBackend.Models;

public partial class Chitietve
{
    public long MaVe { get; set; }

    public string LoaiVe { get; set; } = null!;

    public string MaChuyenBay { get; set; } = null!;

    public int SoLuong { get; set; }

    public string TinhTrang { get; set; } = null!;

    public decimal TongGia { get; set; }

    public virtual Chuyenbay MaChuyenBayNavigation { get; set; } = null!;

    public virtual Ve MaVeNavigation { get; set; } = null!;
}
