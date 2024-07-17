using System;
using System.Collections.Generic;

namespace BookingTourWeb_WebAPI.Models;

public partial class Chuyenbay
{
    public string MaChuyenBay { get; set; } = null!;

    public string MaMayBay { get; set; } = null!;

    public TimeSpan GioBay { get; set; }

    public string NoiXuatPhat { get; set; } = null!;

    public string NoiDen { get; set; } = null!;

    public DateTime NgayXuatPhat { get; set; }

    public decimal DonGia { get; set; }

    public int SoLuongVeBsn { get; set; }

    public int SoLuongVeEco { get; set; }

    public virtual ICollection<Chitietve> Chitietves { get; set; } = new List<Chitietve>();

    public virtual Maybay MaMayBayNavigation { get; set; } = null!;
}
