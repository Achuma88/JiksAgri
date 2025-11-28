using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiksAgriFarm.Data.Models.Domain
{
    public class UpdateStock
    {
        public int ProductID { get; set; }
        public int NewStock {  get; set; }
    }
}
