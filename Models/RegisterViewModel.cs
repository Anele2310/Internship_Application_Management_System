using System.ComponentModel.DataAnnotations;

namespace Internship_Application_Management_System.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Email {  get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
