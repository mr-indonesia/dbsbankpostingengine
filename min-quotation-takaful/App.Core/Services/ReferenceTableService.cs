using App.Core.Interfaces;
using App.Core.Models;
using AutoMapper;
using DataAccess.EFCore.Repositories;
using DataAccess.EFCore;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using App.Core.Models.Users;

namespace App.Core.Services
{
    public class ReferenceTableService : EFRepository, IReferenceTableService
    {
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        public ReferenceTableService(ApplicationContext context, IRepository repository, IMapper mapper) : base(context)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        public ReferenceTableService(ApplicationContext context, IRepository repository) : base(context)
        {
            this._repository = repository;
        }

        public async Task<List<DdlGroupProduct>> FindDllProductGroup()
        {
            try
            {
                string strQuery = $@"SELECT Code, Value = Code
                                    ,Name = Descr
                                    ,Code + ' : ' + Descr AS ListBoxDisplay 
                                    FROM MstProductGroup
									";
                var data = await _repository.QueryAsync<DdlGroupProduct>(strQuery, null);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<DdlRoleAccess>> FindDdlRoleAcces()
        {
            try
            {
                string strQuery = $@"SELECT Value = RoleAccessID
                                    ,Name = RoleAccessName 
                                    ,RoleAccessName AS ListBoxDisplay 
                                    FROM [QuotationNonHealth].[dbo].[RoleAccess]
									";
                var data = await _repository.QueryAsync<DdlRoleAccess>(strQuery, null);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<DdlAmount>> FindDdlAmount()
        {
            try
            {
                string strQuery = $@"SELECT Value = Code
                                    ,Name = Description 
                                    ,Description AS ListBoxDisplay 
                                    FROM [QuotationNonHealth].[dbo].[WFAmount]
									";
                var data = await _repository.QueryAsync<DdlAmount>(strQuery, null);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<DdlUser>> FindDdlUser()
        {
            try
            {
                string strQuery = $@"SELECT usr.UserCode,  Value = usr.UserCode
                                    ,ListBoxDisplay = ISNULL(usr.FrontName,' ') + ISNULL(usr.MiddleName,' ') + ISNULL(usr.LastName,' ') + ' (' + rl.Description + ')'
                                    FROM MstUsers usr
                                    INNER JOIN MstRoles rl ON usr.RoleCode = rl.RoleCode
									";
                var data = await _repository.QueryAsync<DdlUser>(strQuery, null);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<DdlCompanyType>> FindDdlCompanyType()
        {
            try
            {
                string strQuery = $@"SELECT Value = CO.Code,
                                    Name = CO.Descr
                                    ,ListBoxDisplay = CO.Descr
                                    FROM MstCompanyType CO
									";
                var data = await _repository.QueryAsync<DdlCompanyType>(strQuery, null);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<DdlCompanyLOB>> FindDdlCompanyLOB()
        {
            try
            {
                string strQuery = $@"SELECT Value = CO.Code,
                                    Name = CO.Descr
                                    ,ListBoxDisplay = CO.Descr
                                    FROM MstCompanyLOB CO
                                    WHERE CO.IsDeleted = 0
									";
                var data = await _repository.QueryAsync<DdlCompanyLOB>(strQuery, null);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<DdlCompanyCategory>> FindDdlCompanyCategory()
        {
            try
            {
                string strQuery = $@"SELECT Value = CAT.Code,
                                    Name = CAT.Descr
                                    ,ListBoxDisplay = CAT.Descr
                                    FROM MstCompanyCategory CAT
                                    WHERE CAT.IsDeleted = 0
									";
                var data = await _repository.QueryAsync<DdlCompanyCategory>(strQuery, null);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<DdlCompanyStatus>> FindDdlCompanyStatus()
        {
            try
            {
                string strQuery = $@"SELECT Value = CS.Code,
                                    Name = CS.Descr
                                    ,ListBoxDisplay = CS.Descr
                                    FROM MstCompanyStatus CS
									";
                var data = await _repository.QueryAsync<DdlCompanyStatus>(strQuery, null);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<DdlCompanyStatus>> FindDdlCompanyStatus(string code)
        {
            try
            {
                string strQuery = $@"SELECT Value = CS.Code,
                                    Name = CS.Descr
                                    ,ListBoxDisplay = CS.Descr
                                    FROM MstCompanyStatus CS
                                    WHERE CS.Code = @StatusCode
									";

                var param = new Dictionary<string, object>
                {
                    {"@StatusCode", code }
                };

                var data = await _repository.QueryAsync<DdlCompanyStatus>(strQuery, param);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<DdlPropinsi>> FindDdlPropinsi()
        {
            try
            {
                string strQuery = $@"SELECT Value = A.Code,
                                    Name = A.Descr
                                    ,ListBoxDisplay = A.Descr
                                    FROM MstPropinsi A
									";
                var data = await _repository.QueryAsync<DdlPropinsi>(strQuery, null);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
