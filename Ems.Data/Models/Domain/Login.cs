using System.ComponentModel.DataAnnotations;

namespace Ems.Data.Models.Domain
{
    public class Login
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        public string EmpEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string EmpPassword { get; set; }
    }
}
