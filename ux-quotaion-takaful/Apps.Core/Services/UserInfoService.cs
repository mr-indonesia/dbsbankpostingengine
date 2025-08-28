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
using Microsoft.Data.SqlClient;
using System.Data;

namespace Apps.Core.Services
{
    public class UserInfoService : EFRepository, IUserInfoService
    {
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        public UserInfoService(ApplicationContext context, IRepository repository, IMapper mapper) : base(context)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        public UserInfoService(ApplicationContext context, IRepository repository) : base(context)
        {
            this._repository = repository;
        }

        public async Task<List<UserDTO>> GetAllUser(string search)
        {
            try
            {
                string query = $@"SELECT A.UserCode, A.FrontName, A.MiddleName, A.LastName
                            ,A.BirthOfDate, A.EmployeeId, A.Email, A.Upliner
                            ,A.PositionTitle, A.IsLocked, A.Active 
                            FROM MstUsers A WITH(NOLOCK)
                            WHERE A.UserCode LIKE '%' + @Search + '%' 
                            OR Email LIKE '%' + @Search + '%' 
                            OR A.FrontName LIKE '%' + @Search + '%' 
                            OR A.MiddleName LIKE '%' + @Search + '%' 
                            OR A.LastName LIKE '%' + @Search + '%' 
                            OR A.Email LIKE '%' + @Search + '%' ";
                var param = new Dictionary<string, object> {
                    { "@Search", search}
                };

                var data = await _repository.QueryListAsync<UserDTO>(query, param, false);

                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserDTO> FindUserinfoById(string id)
        {
            try
            {
                string query = $@"SELECT A.UserCode, A.BranchCode, A.FrontName, A.MiddleName, A.LastName
                                ,A.BirthOfDate, A.EmployeeId, A.Email, A.Upliner
                                ,A.PositionTitle, A.IsLocked, A.Active 
                                FROM MstUsers A WITH(NOLOCK)
                                WHERE A.UserCode = @UserId";
                var param = new Dictionary<string, object> {
                    { "@UserId", id}
                };

                var data = (await _repository.QueryListAsync<UserDTO>(query, param, false)).FirstOrDefault();

                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

		public async Task<bool> UpdateUserInfo(UserDTO model)
		{
			try
			{
                string query = $@"UPDATE [dbo].[MstUsers]
                                SET [BranchCode] = @BranchCode
                                ,[FrontName] = @FrontName
                                ,[Email] = @Email
                                ,[MiddleName] = @MiddleName
                                ,[LastName] = @LastName
                                --,[PositionTitle] = @PositionTitle
                                WHERE [UserCode] = @UserName";

				var param = new Dictionary<string, object> {
					{ "@BranchCode", model.BranchCode},
					{ "@FrontName", model.FrontName},
					{ "@MiddleName", model.MiddleName},
					{ "@LastName", model.LastName},
					{ "@Email", model.Email},
					{ "@PositionTitle", model.PositionTitle},
					{ "@UserName", model.UserCode},
				};
				await _repository.ExecuteQueryAsync(query, param);

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
