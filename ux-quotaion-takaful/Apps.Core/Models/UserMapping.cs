using DataAccess.EFCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Models
{
    public class UserMapping
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string BranchCode { get; set; }

        // Navigation properties
        public User User { get; set; }
        public CompanyBranch Branch { get; set; }
    }
}
