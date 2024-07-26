using System;
using System.Collections.Generic;

namespace BookingTourWeb_WebAPI.Models
{
    public partial class Ve
    {
        public Ve()
        {
            Chitietves = new HashSet<Chitietve>();
            Hoadons = new HashSet<Hoadon>();
        }

        public long MaVe { get; set; }
        public long MaKh { get; set; }
        public DateTime NgayDatVe { get; set; }

        public virtual Khachhang MaKhNavigation { get; set; } = null!;
        public virtual ICollection<Chitietve> Chitietves { get; set; }
        public virtual ICollection<Hoadon> Hoadons { get; set; }
    }
}
