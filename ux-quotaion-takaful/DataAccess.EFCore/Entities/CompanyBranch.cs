using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EFCore.Entities
{
    public class CompanyBranch
    {
        public string BranchCode { get; set; }
        public string CompanyCode { get; set; }
        public string BranchName { get; set; }
        public string Address1 { get; set; }
        public string Address12 { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        // Navigation properties
        //public Company Company { get; set; }
        //public ICollection<User> Users { get; set; }
        //public ICollection<UserMapping> UserMappings { get; set; }
    }
}
