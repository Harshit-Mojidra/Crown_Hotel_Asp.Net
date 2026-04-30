using System;

namespace hrbs_project.Models
{
    public class BookingViewModel
    {
        public int RoomId { get; set; }

        public string RoomName { get; set; } = "";
        public string Image { get; set; } = "";
        public decimal Price { get; set; }

        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public decimal TotalPrice { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
    }
}