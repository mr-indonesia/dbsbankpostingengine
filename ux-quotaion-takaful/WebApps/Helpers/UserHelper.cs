using Apps.Core.Interfaces;
using Apps.Core.Models;
using System;
using System.Drawing.Text;
using WebApps.Controllers;

namespace WebApps
{
	public class UserHelper : BaseController
	{
		public UserHelper(IUnitOfWork unitOfWork) : base(unitOfWork)
		{
		}

		//public static bool FindUserRole(string userid, string roleid)
		//{
		//	IUnitOfWork unitOfWork;
		//	var cls = new UserHelper(unitOfWork);
		//	return false;
		//}

		public bool GetUserRole(string userid, string roleid)
		{
			try
			{
				var roles = _unitOfWOrk.RoleAccessService.GetRoleAccesByUserId(userid).ConfigureAwait(true).GetAwaiter().GetResult();
				foreach (var role in roles)
				{
					if(role.RoleCode == roleid)
					{
						return true;
					}
				}
			}
			catch (Exception)
			{

				return false;
			}
			return false;
		}
	}
}
