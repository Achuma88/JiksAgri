using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiksAgriFarm.Data.Models.Domain
{
    public class Admin
    {
        [Key]
        public int AdminID { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string AdminName { get; set; }
        [Required(ErrorMessage = "Surname is required")]
        public string AdminSurname { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string AdminEmail { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        public string AdminPhone { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string AdminPassword { get; set; }
        public string AdminStatus { get; set; }
    }

}
