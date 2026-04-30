using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hrbs_project.Models
{
    [Table("booking_order")]
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        public int user_id { get; set; }
        public int room_id { get; set; }

        public DateTime check_in { get; set; }
        public DateTime check_out { get; set; }

        public string? status { get; set; }
        public string? order_id { get; set; }

        public string? user_name { get; set; }
        public string? phone { get; set; }
        public string? room_name { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal price { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal paid_amount { get; set; }

        public string? payment_id { get; set; }

        public DateTime booking_date { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(10,2)")]
        public decimal? refund_amount { get; set; }
        public DateTime? refund_date { get; set; }

        public string? refund_status { get; set; } = "Pending";
    }
}