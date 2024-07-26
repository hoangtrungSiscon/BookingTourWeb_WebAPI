using System;
using System.Collections.Generic;

namespace BookingTourWeb_WebAPI.Models
{
    public partial class Taikhoan
    {
        public Taikhoan()
        {
            Khachhangs = new HashSet<Khachhang>();
        }

        public long MaTaiKhoan { get; set; }
        public int VaiTro { get; set; }
        public string TaiKhoan1 { get; set; } = null!;
        public string MatKhau { get; set; } = null!;

        public virtual ICollection<Khachhang> Khachhangs { get; set; }
    }
}
