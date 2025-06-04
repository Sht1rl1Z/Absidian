using System.ComponentModel.DataAnnotations;

namespace WebApplication39.Models
{
    public class UserRegModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
