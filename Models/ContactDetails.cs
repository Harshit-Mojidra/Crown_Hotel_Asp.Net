using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("contact_details")]
public class ContactDetails
{
    [Key]
    public int id { get; set; }

    public string address { get; set; }
    public string phone1 { get; set; }
    public string phone2 { get; set; }
    public string email { get; set; }
}