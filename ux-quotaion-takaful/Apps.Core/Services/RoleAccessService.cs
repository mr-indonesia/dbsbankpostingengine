using Apps.Core.Interfaces;
using AutoMapper;
using DataAccess.EFCore.Interfaces;
using DataAccess.EFCore;
using DataAccess.EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Core.DTOS;
using Apps.Core.Models.RoleAccess;
using DataAccess.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using Apps.Core.Models.Users;

namespace Apps.Core.Services
{
	public class RoleAccessService : EFRepository, IRoleAccessService
	{
		private readonly IMapper _mapper;
		private readonly IRepository _repository;

		public RoleAccessService(ApplicationContext context, IRepository repository) : base(context)
		{
			this._repository = repository;
		}
		public RoleAccessService(ApplicationContext context, IRepository repository, IMapper mapper) : base(context)
		{
			this._repository = repository;
			this._mapper = mapper;
		}

		public async Task<List<RoleAccessViewModel>> GetAllRoleAccess(string search)
		{
			try
			{
				string query = $@"SELECT A.RoleAccessID, A.RoleAccessName
								,A.IsDeleted, A.CreatedBy, A.CreatedAt
								,A.ModifiedBy, A.ModifiedAt
								FROM RoleAccess A WITH(NOLOCK)
								WHERE A.RoleAccessName LIKE '%' + @Search + '%' 
								OR A.RoleAccessID LIKE '%' + @Search + '%'  ";
				var param = new Dictionary<string, object> {
					{ "@Search", search}
				};

				var data = await _repository.QueryListAsync<RoleAccessViewModel>(query, param, false);

				return data;
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<List<UserModel.RoleAccessViewModel>> GetRoleAccesByUserId(string userid)
		{
			try
			{
				string query = $@"SELECT 
								RoleCode = A.RoleApplicationId
								,RoleName = B.RoleAccessName
								,UserCode = A.UserId
								,UserName = ISNULL(C.FrontName,' ') + ISNULL(C.MiddleName,' ') + ISNULL(C.LastName,' ')
								FROM RoleApplication A WITH(NOLOCK)
								INNER JOIN RoleAccess B WITH(NOLOCK) ON A.RoleApplicationId = B.RoleAccessID
								INNER JOIN MstUsers C WITH(NOLOCK) ON A.UserId = C.UserCode
								WHERE A.UserId = @Username  ";

				var param = new Dictionary<string, object> {
					{ "@Username", userid}
				};

				var data = await _repository.QueryListAsync<UserModel.RoleAccessViewModel>(query, param, false);

				return data;
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<RoleAccessViewModel> CreateRoleAccess(RoleAccessViewModel model, string userId)
		{
            RoleAccessViewModel dtReturn = null;
            int intReturn = 0;
            try
            {
				//setting entities role access
				RoleAccess roleAccess = new RoleAccess { 
					RoleAccessID = model.RoleAccessID,
					RoleAccessName = model.RoleAccessName,
					IsDeleted = false,
					CreatedAt = DateTime.Now,
					CreatedBy = userId,
					ModifiedAt = null,
					ModifiedBy = null
				};

                if (await context.RoleAccesses.Where(e => e.RoleAccessID == model.RoleAccessID).FirstOrDefaultAsync() != null)
                {
					throw new Exception("Role Access Code Already Exists");
                }

                await context.AddAsync(roleAccess);
                intReturn = await context.SaveChangesAsync();

                if (intReturn != 0)
                    dtReturn = model;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return dtReturn;
        }

		public async Task<RoleAccess> FindRoleAccess(string rolecode)
		{
			var roleAccess = await context.RoleAccesses.Where(e => e.RoleAccessID == rolecode).FirstOrDefaultAsync();
			return roleAccess;
		}

		public async Task<RoleAccess> UpdateRoleAccess(RoleAccess data)
		{
			context.RoleAccesses.Update(data);
			await context.SaveChangesAsync();

			return data;
		}

		public async Task<bool> DeleteRoleAccess(RoleAccess data)
		{
			context.RoleAccesses.Remove(data);
			await context.SaveChangesAsync();
			return true;
		}


	}
}
