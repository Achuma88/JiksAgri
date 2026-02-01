using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiksAgriFarm.Data.Models.Domain
{
    public class RecentFarmer
    {
        public int FarmerID { get; set; }
        public string FarmerName { get; set; }
        public string FarmerStatus { get; set; }
        public DateTime RegisteredDate { get; set; }
    }
}
