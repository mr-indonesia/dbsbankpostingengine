using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Models.Users
{
	public class UserModel { 
		public partial class LoginViewModel {
			public string UserName { get; set; }
			public string PasswordHash { get; set; }
			public bool RememberMe { get; set; }
			public string ReturnUrl { get; set; }
			public string RoleId { get; set; }
			public string JWTToken { get; set; }
			public bool? UseJwt { get; set; }
			public string AppID { get; set; }
			public string PassportUrl { get; set; }
		}

        public partial class  UserRequest
        {
			public Guid UserId { get; set; }
			public string Username { get; set; }
			public string FirstName { get; set; }
			public string LastName { get; set; }
			public string FullName { get; set; }
			public string RoleCode { get; set; }
			public string Password { get; set; }
			public string Email { get; set; }
			public bool IsLocked { get; set; }
			public bool Active { get; set; }
		}

        public partial class RoleAccessViewModel { 
			public string RoleCode { get; set; }
			public string RoleName { get; set; }
			public string UserCode { get; set; }
			public string UserName { get; set; }
		}
	}
}
