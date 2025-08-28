using Apps.Core.DTOS;
using Apps.Core.Interfaces;
using AutoMapper;
using DataAccess.EFCore;
using DataAccess.EFCore.Interfaces;
using DataAccess.EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Services
{
    public class UserMappingService : EFRepository, IUserMappingService
    {
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        public UserMappingService(ApplicationContext context, IRepository repository, IMapper mapper) : base(context)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        public UserMappingService(ApplicationContext context, IRepository repository) : base(context)
        {
            this._repository = repository;
        }

        public async Task<List<UserDTO>> GetAllUserMapping()
        {
            try
            {
                string query = $@"SELECT DISTINCT
                                    UserCode = A.UserId
                                    ,dbo.funcGetRoleAccess(A.UserId) as RoleApplication
                                    ,C.FrontName, C.MiddleName, C.LastName
                                    ,FullName = ISNULL(C.FrontName,' ') + ISNULL(C.MiddleName,' ') + ISNULL(C.LastName,' ')
                                    ,C.BirthOfDate, C.EmployeeId, C.Email
                                    ,C.Upliner, C.BranchCode, C.PositionTitle 
                                    FROM RoleApplication A WITH(NOLOCK)
                                    INNER JOIN RoleAccess B WITH(NOLOCK) ON A.RoleApplicationId = B.RoleAccessID
                                    INNER JOIN MstUsers C WITH(NOLOCK) ON A.UserId = C.UserCode";

                var data = await _repository.QueryListAsync<UserDTO>(query, null, false);

                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<Result<UserMappingResponse>> CreateUserMappingAsync(UserMappingRequest request)
        {
            //var user = await context.Users.FindAsync(request.UserId);
            //if (user == null)
            //{
            //    //retun user not found
            //}

            //// Check if branch exists
            //var branch = await context.CompanyBranches.FindAsync(request.BranchCodes)
            //if (branch == null)
            //{
            //    //retun user not found
            //}
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserMappingAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserMappingResponse> GetUserMappingAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserMappingResponse> UpdateUserMappingAsync(string userId, UserMappingRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
