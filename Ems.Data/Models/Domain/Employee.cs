using System.ComponentModel.DataAnnotations;

namespace Ems.Data.Models.Domain
{
    public class Employee
    {
        public int EmpID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string EmpName { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        public string EmpSurname { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string EmpEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string EmpPhone { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string EmpPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("EmpPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string EmpRole { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string EmpStatus { get; set; }
    }
}

