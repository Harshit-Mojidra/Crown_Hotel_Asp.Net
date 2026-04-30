using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hrbs_project.Models
{
    [Table("carousel")]
    public class Carousel
    {
        [Key]
        public int id { get; set; }

        public string image { get; set; } = string.Empty;
    }
}
