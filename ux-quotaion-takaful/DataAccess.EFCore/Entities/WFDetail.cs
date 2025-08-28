using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EFCore.Entities
{
    public class WFDetail
    {
        public string HeaderID { get; set; }
        public int State { get; set; }
        public string RoleID { get; set; }
        public string Name { get; set; }
        public string AmountID { get; set; }

    }
}
