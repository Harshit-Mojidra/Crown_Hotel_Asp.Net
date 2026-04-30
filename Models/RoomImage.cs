using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hrbs_project.Models
{
    [Table("room_images")]
    public class RoomImage
    {
        [Key]
        public int id { get; set; }

        public int room_id { get; set; }

        public string image { get; set; } = string.Empty;
    }
}
