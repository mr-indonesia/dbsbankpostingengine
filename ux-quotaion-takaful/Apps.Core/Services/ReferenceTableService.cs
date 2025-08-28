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
using Apps.Core.Models.Company;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Apps.Core.Services
{
    public class ReferenceTableService : EFRepository, IReferenceTableService
    {
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        private readonly IApiService _httpClientService;
        public ReferenceTableService(ApplicationContext context, IRepository repository, IApiService httpClientService) : base(context)
        {
            this._repository = repository;
            _httpClientService = httpClientService;
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
                var data = await _repository.QueryListAsync<DdlGroupProduct>(strQuery, null);
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
                var data = await _repository.QueryListAsync<DdlRoleAccess>(strQuery, null);
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
                var data = await _repository.QueryListAsync<DdlAmount>(strQuery, null);
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
                var data = await _repository.QueryListAsync<DdlUser>(strQuery, null);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<DdlCompanyType>> FindDdlCompanyType(string url)
        {
            try
            {
                var httpResponse = await _httpClientService.GetAsync(url);

                if (httpResponse.Success)
                {
                    JObject finalResult = JObject.Parse(httpResponse.Returns);
                    //_httpClientService.SetToken(finalResult["data"]["token"].ToString());
                    var data = JsonConvert.DeserializeObject<List<DdlCompanyType>>(finalResult["Data"].ToString());
                    return data;
                }
                else
                {
                    return new List<DdlCompanyType>();
                }
            }
            catch (Exception)
            {

                return new List<DdlCompanyType>();
            }
        }

		public async Task<List<DdlCompanyLOB>> FindDdlCompanyLOB(string url)
		{
			try
			{
				var httpResponse = await _httpClientService.GetAsync(url);

				if (httpResponse.Success)
				{
					JObject finalResult = JObject.Parse(httpResponse.Returns);
					//_httpClientService.SetToken(finalResult["data"]["token"].ToString());
					var data = JsonConvert.DeserializeObject<List<DdlCompanyLOB>>(finalResult["Data"].ToString());
					return data;
				}
				else
				{
					return new List<DdlCompanyLOB>();
				}
			}
			catch (Exception)
			{

				return new List<DdlCompanyLOB>();
			}
		}

		public async Task<List<DdlCompanyCategory>> FindDdlCompanyCategory(string url)
		{
			try
			{
				var httpResponse = await _httpClientService.GetAsync(url);

				if (httpResponse.Success)
				{
					JObject finalResult = JObject.Parse(httpResponse.Returns);
					//_httpClientService.SetToken(finalResult["data"]["token"].ToString());
					var data = JsonConvert.DeserializeObject<List<DdlCompanyCategory>>(finalResult["Data"].ToString());
					return data;
				}
				else
				{
					return new List<DdlCompanyCategory>();
				}
			}
			catch (Exception)
			{

				return new List<DdlCompanyCategory>();
			}
		}

		public async Task<List<DdlCompanyStatus>> FindDdlCompanyStatus(string url)
		{
			try
			{
				var httpResponse = await _httpClientService.GetAsync(url);

				if (httpResponse.Success)
				{
					JObject finalResult = JObject.Parse(httpResponse.Returns);
					//_httpClientService.SetToken(finalResult["data"]["token"].ToString());
					var data = JsonConvert.DeserializeObject<List<DdlCompanyStatus>>(finalResult["Data"].ToString());
					return data;
				}
				else
				{
					return new List<DdlCompanyStatus>();
				}
			}
			catch (Exception)
			{

				return new List<DdlCompanyStatus>();
			}
		}

		public async Task<List<DdlPropinsi>> FindDdlPropinsi(string url)
		{
			try
			{
				var httpResponse = await _httpClientService.GetAsync(url);

				if (httpResponse.Success)
				{
					JObject finalResult = JObject.Parse(httpResponse.Returns);
					//_httpClientService.SetToken(finalResult["data"]["token"].ToString());
					var data = JsonConvert.DeserializeObject<List<DdlPropinsi>>(finalResult["Data"].ToString());
					return data;
				}
				else
				{
					return new List<DdlPropinsi>();
				}
			}
			catch (Exception)
			{

				return new List<DdlPropinsi>();
			}
		}
	}
}
