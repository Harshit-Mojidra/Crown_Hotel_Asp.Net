using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace hrbs_project.Models;

[Table("user_queries")]
public class UserQuery
{
    [Key]
    public int id { get; set; }

    public string name { get; set; }
    public string email { get; set; }
    public string message { get; set; }
}