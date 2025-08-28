using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EFCore.Entities
{
    public class UserBranch
    {
        public string UserId { get; set; }
        public string BranchCode { get; set; }
        public User User { get; set; }
        public CompanyBranch Branch { get; set; }
    }
}
