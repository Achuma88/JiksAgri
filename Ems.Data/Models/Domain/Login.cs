using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JiksAgriFarm.Data.Models.Domain
{
    public class Login
    {
        //[Key]
        //public int CustomerID { get; set; }
        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string CustomerPassword { get; set; }
    }
}
