using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiksAgriFarm.Data.Models.Domain
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Surname is required")]
        public string CustomerSurname { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        public string CustomerPhone { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string CustomerPassword { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("CustomerPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
        public DateTime DateJoined { get; set; }
        public string CustomerStatus { get; set; }
    }
}
