using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiksAgriFarm.Data.Models.Domain
{
    public class AdminLogin
    {
        [Required]
        [EmailAddress]
        public string AdminEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string AdminPassword { get; set; }
    }
}
