using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hrbs_project.Models
{
    [Table("settings")] 
    public class Settings
    {
        [Key]
        public int id { get; set; }

        [Column("site_title")]
        public string SiteTitle { get; set; } = string.Empty;

        [Column("site_about")]
        public string About { get; set; } = string.Empty;

        public bool IsShutdown { get; set; }

        public string Address { get; set; } = string.Empty;

        public string GoogleMap { get; set; } = string.Empty;

        public string Phone1 { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Facebook { get; set; } = string.Empty;
        public string? Iframe { get; set; } = string.Empty;

        [NotMapped]
        public List<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
    }
}