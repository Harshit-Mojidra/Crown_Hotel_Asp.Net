using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hrbs_project.Models
{
    [Table("rooms")]
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public string? Area { get; set; }

        public string Features { get; set; } = string.Empty;

        public string Facilities { get; set; } = string.Empty;

        public int? Quantity { get; set; }

        public string Image { get; set; } = string.Empty;

        public int? Adult { get; set; }

        public int? Children { get; set; }  

        public bool IsAvailable { get; set; }

        public string? SelectedFeatures { get; set; }
        public string? SelectedFacilities { get; set; } 
    }
}