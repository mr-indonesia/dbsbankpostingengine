using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EFCore.Entities
{
	public class RoleAccess
	{
		public string RoleAccessID { get; set; }
		public string RoleAccessName { get; set; }
		public bool IsDeleted { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedAt { get; set; }
		public string ModifiedBy { get; set; }
		public DateTime? ModifiedAt { get; set; }

	}

}
