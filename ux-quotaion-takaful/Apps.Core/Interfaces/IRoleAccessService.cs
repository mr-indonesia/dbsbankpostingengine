using Apps.Core.Models.RoleAccess;
using Apps.Core.Models.Users;
using DataAccess.EFCore.Entities;
using DataAccess.EFCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Interfaces
{
	public interface IRoleAccessService : IRepository
	{
		Task<List<RoleAccessViewModel>> GetAllRoleAccess(string search);
		Task<List<UserModel.RoleAccessViewModel>> GetRoleAccesByUserId(string userid);
		Task<RoleAccessViewModel> CreateRoleAccess(RoleAccessViewModel model, string userId);
		Task<RoleAccess> FindRoleAccess(string rolecode);
		Task<RoleAccess> UpdateRoleAccess(RoleAccess data);
		Task<bool> DeleteRoleAccess(RoleAccess data);
	}
}
