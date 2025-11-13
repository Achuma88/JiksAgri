using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ems.Data.Models.Domain
{
    public class EmployeeSummary
    {
        public int TotalEmployees { get; set; }
        public int ActiveEmployees { get; set; }
        public int InactiveEmployees { get; set; }
        public int TotalAdmins { get; set; }
        public int TotalManagers { get; set; }
        public int TotalStaff { get; set; }
    }

}
