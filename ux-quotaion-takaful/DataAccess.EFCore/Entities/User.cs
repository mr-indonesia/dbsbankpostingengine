using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EFCore.Entities
{
    public class User
    {
        public string UserCode { get; set; }
        public string Password { get; set; }
        public string RoleCode { get; set; }
        public string FrontName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthOfDate { get; set; }
        public string EmployeeId { get; set; }
        public string Email { get; set; }
        public string Upliner { get; set; }
        public string BranchCode { get; set; }
        public string PositionTitle { get; set; }
        public byte[] Signature { get; set; }
        public byte[] Fhoto { get; set; }
        public bool? IsLocked { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastChangePwd { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        // Navigation properties
        //public Role Role { get; set; }
        //public CompanyBranch Branch { get; set; }
        //public ICollection<UserRoleApplication> UserRoleApplications { get; set; }
        //public ICollection<UserMapping> UserMappings { get; set; }

    }
}
