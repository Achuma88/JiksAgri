using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiksAgriFarm.Data.Models.Domain
{
    public class Farmer
    {
        [Key]
        public int FarmerID { get; set; }
        public string FarmerName { get; set; }
        public string FarmerPhone { get; set; }
        public string FarmerEmail { get; set; }
        public string FarmerPassword { get; set; }
        public string FarmerLocation { get; set; }
        public DateTime RegisteredDate { get; set; }
        public string FarmerStatus { get; set; }
        public bool IsVerified { get; set; }
    }
}
