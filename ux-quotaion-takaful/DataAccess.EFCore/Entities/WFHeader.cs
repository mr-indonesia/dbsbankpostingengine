using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EFCore.Entities
{
    public class WFHeader
    {
        public string HeaderID { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string DataAreaID { get; set; }
        public string COA { get; set; }
        public string CategoryTypeID { get; set; }
        public bool IsDedicatedRequester { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }

    }
}
