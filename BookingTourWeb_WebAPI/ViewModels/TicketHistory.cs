using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BookingTourWeb_WebAPI.ViewModels
{
    public class TicketHistory
    {
        [Key]
        public long idKH { get; set; }
        public long idTicket { get; set; }
        public DateTime ngaydatve { get; set; }
        public string noidi { get; set; }
        public string noiden { get; set; }
        public string loai { get; set; }
        public long soluong {  get; set; }
        public DateTime ngaydi { get; set; }
        public string machuyen { get; set; }
        public decimal gia { get; set; }
        public string tinhtrang { get; set; }

    }
}
