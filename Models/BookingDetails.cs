using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("booking_details")]
public class BookingDetails
{
    [Key]
    public int id { get; set; }

    public string? order_id { get; set; }
    public string? room_name { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? price { get; set; }
}