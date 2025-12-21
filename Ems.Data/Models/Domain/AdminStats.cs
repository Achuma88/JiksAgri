using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiksAgriFarm.Data.Models.Domain
{
    public class AdminStats
    {
        public int TotalFarmers { get; set; }
        public int ApprovedFarmers { get; set; }
        public int PendingFarmers { get; set; }
        public int InactiveFarmers { get; set; }

        public int TotalAdmins { get; set; }
    }

}
