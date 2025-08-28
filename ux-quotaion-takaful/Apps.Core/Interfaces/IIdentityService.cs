using Apps.Core.Models.Authentication;
using Apps.Core.Models.Users;
using DataAccess.EFCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Interfaces
{
	public interface IIdentityService : IRepository
	{
		Task<AuthenticationResult> AuthecticateAsync(UserModel.UserRequest userReq);
	}
}
