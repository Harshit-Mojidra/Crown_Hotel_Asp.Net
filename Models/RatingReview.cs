using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hrbs_project.Models
{
    [Table("rating_review")]
    public class RatingReview
    {
        [Key]
        public int id { get; set; }

        public int user_id { get; set; }

        public int room_id { get; set; }

        [Range(1, 5)]
        public int rating { get; set; }

        public string? review { get; set; }

        [DataType(DataType.Date)]
        public DateTime? review_date { get; set; }

        public bool is_read { get; set; }

        // Navigation properties (nullable to fix CS8618)
        [NotMapped]
        public string? UserName { get; set; }

        [NotMapped]
        public string? RoomName { get; set; }
    }
}