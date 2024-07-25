using System;
using System.Collections.Generic;

namespace BookingTourWeb_WebAPI.Models;

public partial class Ve
{
    public long MaVe { get; set; }

    public long MaKh { get; set; }

    public DateTime NgayDatVe { get; set; }

    public virtual ICollection<Chitietve> Chitietves { get; set; } = new List<Chitietve>();

    public virtual ICollection<Hoadon> Hoadons { get; set; } = new List<Hoadon>();

    public virtual Khachhang MaKhNavigation { get; set; } = null!;
}
