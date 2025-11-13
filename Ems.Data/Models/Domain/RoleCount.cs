using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ems.Data.Models.Domain
{
    public class RoleCount
    {
        public string EmpRole {  get; set; }    
        public int Count { get; set; }
    }
}
