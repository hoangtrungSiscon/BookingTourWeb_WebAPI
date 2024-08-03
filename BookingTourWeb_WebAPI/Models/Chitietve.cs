using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace BookingTourWeb_WebAPI.Models
{
    public partial class Chitietve
    {
        public long MaCtv { get; set; }
        public long MaVe { get; set; }
        public string LoaiVe { get; set; } = null!;
        public string MaChuyenBay { get; set; } = null!;
        public int SoLuong { get; set; }
        public string TinhTrang { get; set; } = null!;
        public decimal TongGia { get; set; }
        [AllowNull]
        [JsonIgnore]
        public virtual Chuyenbay? MaChuyenBayNavigation { get; set; } = null!;
        [AllowNull]
        [JsonIgnore]
        public virtual Ve? MaVeNavigation { get; set; } = null!;
    }
}
