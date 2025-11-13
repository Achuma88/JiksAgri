using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ems.Data.Models.Domain
{
    public class StatusCount
    {
        public string EmpStatus { get; set; }
        public int Count { get; set; }
    }
}
