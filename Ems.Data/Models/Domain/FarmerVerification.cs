using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiksAgriFarm.Data.Models.Domain
{
    public class FarmerVerification
    {
        [Key]
        public int VerificationID { get; set; }
        public int FarmerID { get; set; }
        public string DocumentPath { get; set; }
        public DateTime UploadedDate { get; set; }
        public string VerificationStatus { get; set; } // Pending / Approved / Rejected
        
    }
}
