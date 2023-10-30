using System.ComponentModel.DataAnnotations;

namespace BookingTourWeb_WebAPI.Models
{
    public class ThongTinChuyenBay
    {
        [Key]
        public string MaChuyenBay { get; set; } = null!;

        public string MaMayBay { get; set; } = null!;

        public string TenMayBay { get; set; } = null!;

        public TimeSpan GioBay { get; set; }

        public string NoiXuatPhat { get; set; } = null!;

        public string NoiDen { get; set; } = null!;

        public DateTime NgayXuatPhat { get; set; }

        public decimal DonGia { get; set; }

        public int SoLuongVeBsn { get; set; }

        public int SoLuongVeEco { get; set; }
    }
}
