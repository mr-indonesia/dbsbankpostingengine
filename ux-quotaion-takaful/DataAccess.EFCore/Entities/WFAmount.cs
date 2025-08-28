using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EFCore.Entities
{
    public class WFAmount
    {
        public string Code { get; set; }
        public decimal From { get; set; }
        public decimal To { get; set; }
        public string Description { get; set; }

    }
}
