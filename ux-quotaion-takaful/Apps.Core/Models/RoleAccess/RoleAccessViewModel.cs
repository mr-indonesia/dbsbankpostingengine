using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Models.RoleAccess
{
	public class RoleAccessViewModel
	{
		public string RoleAccessID { get; set; }
		public string RoleAccessName { get; set; }
		public bool IsDeleted { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedAt { get; set; }
		public string ModifiedBy { get; set; }
		public DateTime? ModifiedAt { get; set; }
	}

	public class RoleModel
	{
		public string RoleCode { get; set; }
		public string RoleName { get; set; }
	}

	public class UserRoleModel
	{
		public int RoleId { get; set; }
		public Guid UserId { get; set; }
	}
}
