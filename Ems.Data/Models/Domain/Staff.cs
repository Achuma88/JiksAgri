using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ems.Data.Models.Domain
{
    public class Staff
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
    }
}
