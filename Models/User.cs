using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace hrbs_project.Models
{
    [Table("UserCred")]
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Pincode { get; set; }
        public DateTime DOB { get; set; }

        public string? Password { get; set; }

        [NotMapped]
        public string? ConfirmPassword { get; set; }
        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public string? Status { get; set; }

        public string? ResetToken { get; set; }
        public DateTime? TokenExpiry { get; set; }
    }
}