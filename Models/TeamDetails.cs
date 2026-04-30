using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hrbs_project.Models
{
    [Table("team_details")]
    public class TeamDetails
    {
        [Key]
        public int id { get; set; }

        public string name { get; set; }
        public string image { get; set; }
    }
}