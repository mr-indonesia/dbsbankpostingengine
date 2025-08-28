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
using Apps.Core.Models;
using Apps.Core.DTOS;
using DataAccess.EFCore.Entities;

namespace Apps.Core.Services
{
    public class CompanyBranchService : EFRepository, ICompanyBranchService
    {
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        public CompanyBranchService(ApplicationContext context, IRepository repository, IMapper mapper) : base(context)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        public CompanyBranchService(ApplicationContext context, IRepository repository) : base(context)
        {
            this._repository = repository;
        }

        public async Task<CompanyInfo> FindCompanyById(string companyId)
        {
            try
            {
                if (companyId == null)
                {
                    companyId = "DAT";
                }

                string strQuery = $@"SELECT DataAreaId = A.BranchCode
									,DataAreaName = A.BranchName
									,A.BranchCode + ' : '+ A.BranchName AS ListBoxDisplay 
									FROM MstBranch A WITH(NOLOCK)  
									WHERE A.BranchCode = @DataAreaId
									";
                var data = (await _repository.QueryListAsync<CompanyInfo>(strQuery, new Dictionary<string, object> { { "@DataAreaId", companyId } })).First();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<BranchDTO>> FindAllBranch()
        {
            try
            {
                string strQuery = $@"SELECT A.BranchCode, A.BranchName, B.CompanyCode 
                                    FROM MstBranch A WITH(NOLOCK)
                                    INNER JOIN MstCompany B ON A.CompanyCode = B.CompanyCode
									";
                var data = await _repository.QueryListAsync<BranchDTO>(strQuery, null);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<DdlBranch>> FindDdlBranch()
        {
            try
            {
                string strQuery = $@"SELECT B.CompanyCode
                                    ,Value = A.BranchCode
                                    ,Name = A.BranchName
                                    ,ListBoxDisplay = A.BranchCode + ' : ' + A.BranchName
                                    FROM MstBranch A WITH(NOLOCK)
                                    INNER JOIN MstCompany B WITH(NOLOCK) ON A.CompanyCode = B.CompanyCode
									";
                var data = await _repository.QueryListAsync<DdlBranch>(strQuery, null);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<CompanyUserDTO>> FindBranchByUserId(string userId)
        {
            try
            {
                string strQuery = $@"SELECT UserCode, BranchCode 
                                    FROM MstUsers WITH(NOLOCK)
                                    WHERE UserCode = @UserId
									";
                var param = new Dictionary<string, object> {
                    { "@UserId", userId}
                };
                var data = await _repository.QueryListAsync<CompanyUserDTO>(strQuery, param);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
