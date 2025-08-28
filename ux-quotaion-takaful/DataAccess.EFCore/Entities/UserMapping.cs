using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EFCore.Entities
{
    public class UserMapping
    {
		public string RoleApplicationID { get; set; }
		public string UserID { get; set; }
		public bool IsDeleted { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedAt { get; set; }
		public string ModifiedBy { get; set; }
		public DateTime? ModifiedAt { get; set; }


		// Navigation properties
		//public User User { get; set; }
		//public CompanyBranch Branch { get; set; }
	}

	public class UserRoleApplication
    {
        public string UserId { get; set; }
        public string RoleCode { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
