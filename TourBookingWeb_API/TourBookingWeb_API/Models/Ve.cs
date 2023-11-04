using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TourBookingWeb_API.Models
{
    public partial class Ve
    {
        /*public Ve()
        {
            Chitietves = new HashSet<Chitietve>();
        }*/
        public long MaVe { get; set; }
        public long MaKh { get; set; }
        public DateTime NgayDatVe { get; set; }
        [JsonIgnore]
        public virtual Khachhang MaKhNavigation { get; set; } = null!;
        /*public virtual ICollection<Chitietve> Chitietves { get; set; }*/
    }
}
