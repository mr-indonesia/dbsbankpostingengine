using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.DTOS
{
    public class UserDTO
    {
        public string UserCode { get; set; }
        public string BranchCode { get; set; }
        public string FrontName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string RoleApplication { get; set; }
        public DateTime? BirthOfDate { get; set; }
        public string EmployeeId { get; set; }
        public string Email { get; set; }
        public string Upliner { get; set; }
        public string PositionTitle { get; set; }
        public bool IsLocked { get; set; }
        public bool Active { get; set; }

        public List<RoleDTO> Roles { get; set; }
        public BranchDTO Branch { get; set; }
        public CompanyDTO Company { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
