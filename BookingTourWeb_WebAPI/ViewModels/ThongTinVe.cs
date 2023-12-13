using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace BookingTourWeb_WebAPI.ViewModels
{
    public class ThongTinVe
    {
        [Key]
        public long MaVe { get; set; }
        public long MaKH { get; set; }

        public string HoTenKH { get; set; }
        public DateTime NgayDatVe { get; set; }
        public string LoaiVe { get; set; }
        public string MaChuyenBay { get; set; } = null!;
        public long Soluong { get; set; }



        public string TinhTrang { get; set; } = null!;

        public decimal TongGia { get; set; }
    }
}
