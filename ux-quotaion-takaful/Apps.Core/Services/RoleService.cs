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
using DataAccess.EFCore.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Apps.Core.Services
{
    public class RoleService : EFRepository, IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        public RoleService(ApplicationContext context, IRepository repository, IMapper mapper) : base(context)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        public RoleService(ApplicationContext context, IRepository repository) : base(context)
        {
            this._repository = repository;
        }

        public async Task<List<RoleApplicationDTO>> GetRoleApplicationByUserId(string userId, bool isAdmin = false)
        {
            try
            {
                string query = $@"SELECT 
                                A.RoleApplicationId
                                ,RoleName = B.RoleAccessName
                                ,UserCode = A.UserId
                                ,C.FrontName
                                ,C.MiddleName
                                ,C.LastName
                                FROM RoleApplication A WITH(NOLOCK)
                                INNER JOIN RoleAccess B WITH(NOLOCK) ON A.RoleApplicationId = B.RoleAccessID
                                INNER JOIN MstUsers C WITH(NOLOCK) ON A.UserId = C.UserCode
                                WHERE A.UserId = @Username";


                var param = new Dictionary<string, object> {
                    { "@Username", userId}
                };

                var data = await _repository.QueryListAsync<RoleApplicationDTO>(query, param, false);

                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<RoleDTO>> GetAllRole()
        {
            try
            {
                string query = $@"SELECT RoleAccessID, RoleAccessName FROM dbo.RoleAccess WITH(NOLOCK)";

                var data = await _repository.QueryListAsync<RoleDTO>(query, null, false);

                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> RemoveRoleByUserId(string userId)
        {
            try
            {
                

                var roleApplication = await GetRoleApplicationByUserId(userId);
                if(roleApplication == null && roleApplication.Count() < 1)
                {
                    return true;
                }
                else
                {
                    string query = $@"DELETE FROM RoleApplication
                                WHERE UserId = @Username";

                    var param = new Dictionary<string, object> {
                        { "@Username", userId}
                    };
                    await _repository.ExecuteQueryAsync(query, param);
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> AddRoleApplication(string userId, string[] role)
        {
            try
            {


                var roleApplication = await GetRoleApplicationByUserId(userId);
                if (roleApplication == null && roleApplication.Count() < 1)
                {
                    return true;
                }
                else
                {
                    for (int i = 0; i < role.Count(); i++)
                    {
                        string query = $@"INSERT INTO RoleApplication(RoleApplicationID, UserID, IsDeleted, CreatedBy, CreatedAt)
                                          VALUES (@RoleApplicationID, @UserID, 0, 'System', GETDATE())";
                        var param = new Dictionary<string, object> {
                            { "@RoleApplicationID", role[i].ToString()},
                            { "@UserID", userId},
                        };

                        await _repository.ExecuteQueryAsync(query, param);
                    }
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
