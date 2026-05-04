using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hrbs_project.Models
{
    [Table("TeamMembers")]
    public class TeamMember
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? ImagePath { get; set; }
    }
}