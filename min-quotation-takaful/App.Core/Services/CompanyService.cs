using App.Core.Interfaces;
using App.Core.Models;
using App.Core.Models.Company;
using App.Core.Models.ProductGroup;
using App.Core.Models.Users;
using AutoMapper;
using Dapper;
using DataAccess.EFCore;
using DataAccess.EFCore.Repositories;
using SharedKernel.Entities;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace App.Core.Services
{
    public class CompanyService : EFRepository, ICompanyService
    {
        private readonly IRepository _repository;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public CompanyService(ApplicationContext context, IRepository repository) : base(context)
        {
            this._repository = repository;
        }


        public async Task<(bool added, string message)> InsertCompanyAgent(CompanyRequest model)
        {
            try
            {
                //check existing company
                /*string query = @"SELECT CompanyName
                                FROM MstCompanyAgent
                                WHERE CompanyName = @CompanyName
                                AND Npwp = @Npwp
                                AND Propinsi = @Propinsi";

                var param = new Dictionary<string, object>
                {
                    {"@CompanyName", model.CompanyName },
                    {"@Npwp", model.Npwp },
                    {"@Propinsi", model.Propinsi },
                };

                var data = await _repository.QueryAsync<string>(query, param, false);

                if (data != null && data.Count > 0)
                {
                    return (false, "Company name already exists");
                }*/

                var param2 = new Dictionary<string, object>
                {
                    {"@CompanyCode", model.CompanyCode },
                    {"@Agent", model.CreatedBy },
                    {"@CompanyName", model.CompanyName },
                    {"@CompanyType", model.CompanyType },
                    {"@CompanyCategory", model.CategoryCode },
                    {"@CompanyLob", model.LobCode },
                    {"@CompanyAddress", model.CompanyAddress },
                    {"@CompanyAddress2", model.CompanyAddress2 },
                    {"@Kotamadya", model.KotaMadya },
                    {"@Propinsi", model.Propinsi },
                    {"@ZipCode", model.ZipCode },
                    {"@Phone", model.Phone },
                    {"@Fax", model.Fax },
                    {"@Email", model.Email },
                    {"@PIC", model.Pic },
                    {"@PIC2", model.Pic2 },
                    {"@PICTitle", model.PicTitle },
                    {"@NPWP", model.Npwp },
                    {"@UserId", model.CreatedBy }
                };

                await _repository.ExecuteQueryAsync("SP_COMPANY_UPSERT", param2, true);

                string msg = string.IsNullOrEmpty(model.CompanyCode) ? "Insert company agent success" : "Update company agent success";

                return (true, msg);

            }
            catch (Exception ex)
            {
                return (false, ex.Message); ;
            }
        }

        public async Task<(bool added, string message)> ApproveCompanyAgent(CompanyApproveRequest model)
        {
			try
			{
				var param = new Dictionary<string, object>
				{
					{"@CompanyCode", model.CompanyCode },
					{"@StatusCode", model.StatusCode },
					{"@Remark", model.Remark },
					{"@UserId", model.UserId }
				};

				await _repository.ExecuteQueryAsync("SP_CompanyApproval", param, true);

				string msg = string.IsNullOrEmpty(model.Remark) ? "Approve company agent success" : "Reject company agent success";

				return (true, msg);

			}
			catch (Exception ex)
			{
				return (false, ex.Message); ;
			}
		}

        public async Task<(bool added, string message)> RemoveCompanyAgent(CompanyApproveRequest model)
        {
            try
            {
                var param = new Dictionary<string, object>
                {
                    {"@CompanyCode", model.CompanyCode }
                };

                await _repository.ExecuteQueryAsync("SP_CompanyRollback", param, true);

                string msg = "Remove company agent success";

                return (true, msg);

            }
            catch (Exception ex)
            {
                return (false, ex.Message); ;
            }
        }



        public async Task<(bool valid, string message)> ValidateCompanyAgent(CompanyRequest model)
        {
            try
            {
                //check existing company
                string query = @"SELECT CompanyName
                                FROM MstCompanyAgent
                                WHERE 
                                (CompanyName = @CompanyName AND Npwp = @Npwp) -- Validasi minimum: nama dan NPWP
                                OR (CompanyName = @CompanyName AND Npwp = @Npwp AND CompanyAddress = @Address) ";

                var param = new Dictionary<string, object>
                {
                    {"@CompanyName", model.CompanyName },
                    {"@Npwp", model.Npwp },
                    {"@Address", model.CompanyAddress },
                };

                var data = await _repository.QueryAsync<string>(query, param, false);

                if (data != null && data.Count > 0)
                {
                    string errMsg = string.Format("Company name : {0}, Npwp : {1}, Address: {2} already exists", model.CompanyName, model.Npwp, model.CompanyAddress);
                    return (false, errMsg);
                }

                return (true, "Company is available");
            }
            catch (Exception ex)
            {

                return (false, ex.Message); ;
            }
        }

        public async Task<List<CompanyViewModel>> GetSearchCompanyAgents(string userId, CompanySearchViewModel model)
        {
            try
            {
                string query = @"SELECT CO.[CompanyCode]
                                ,CO.[CompanyName]
                                ,CO.[CompanyType]
                                ,CO.[CompanyTypeDescr]
                                ,CO.[CategoryCode]
                                ,CO.[CompanyCategoryDesc]
                                ,CO.[LobCode]
                                ,CO.[CompanyLobDescr]
                                ,CO.[CompanyAddress]
                                ,CO.[CompanyAddress2]
                                ,CO.[KotaMadya]
                                ,CO.[Propinsi]
                                ,CO.[PropinsiDesc]
                                ,CO.[ZipCode]
                                ,CO.[Phone]
                                ,CO.[Fax]
                                ,CO.[Email]
                                ,CO.[Pic]
                                ,CO.[Pic2]
                                ,CO.[PicTitle]
                                ,CO.[RegisterDate]
                                ,CO.[Npwp]
                                ,CO.[StatusCode]
                                ,CO.[StatusDesc]
                                ,CO.[AgentCode]
                                ,CO.[AgentName]
                                ,CO.[ChanelCode]
                                ,CO.[ChannelDesc]
                                ,CO.[SubChannelCode]
                                ,CO.[SubChannelDistDesc]
                                ,CO.[AgentStartDate]
                                FROM [dbo].[V_CompanyRegistration] CO
                                WHERE CO.CreatedBy = @UserId
                                AND Isnull(CO.CompanyCode,'||') = ISNULL(NULLIF(@CompanyCode,''),Isnull(CO.CompanyCode,'||'))
                                AND Isnull(CO.StatusCode,'||') = ISNULL(NULLIF(@StatusCode,''),Isnull(CO.StatusCode,'||'))
                                AND Isnull(CO.AgentCode,'||') = ISNULL(NULLIF(@AgentCode,''),Isnull(CO.AgentCode,'||'))
                                AND Isnull(CO.CategoryCode,'||') = ISNULL(NULLIF(@CategoryCode,''),Isnull(CO.CategoryCode,'||'))
                                AND Isnull(CO.LobCode,'||') = ISNULL(NULLIF(@LobCode,''),Isnull(CO.LobCode,'||'))
                                AND Isnull(CO.CompanyName,'||') = ISNULL(NULLIF(@CompanyName,''),Isnull(CO.CompanyName,'||'))
                                AND convert(date,CO.RegisterDate) >= ISNULL(NULLIF(convert(date,@FromDate),''),CO.RegisterDate) 
                                AND convert(date,CO.RegisterDate) <= ISNULL(NULLIF(convert(date,@ToDate),''),CO.RegisterDate) 
                                ORDER BY CO.CompanyName";

                var param = new Dictionary<string, object>
                {
                    {"@UserId", userId },
                    {"@CompanyCode", model.CompanyCode },
                    {"@StatusCode", model.Status },
                    {"@AgentCode", model.AgentCode },
                    {"@CategoryCode", model.CategoryCode },
                    {"@LobCode", model.LOB },
                    {"@CompanyName", model.CompanyName },                    
                    {"@FromDate", model.FromDate },                    
                    {"@ToDate", model.ToDate },                    
                };

                var data = await _repository.QueryAsync<CompanyViewModel>(query, param, false);

                return data;
            }
            catch (Exception)
            {

                return new List<CompanyViewModel>();
            }
        }

        public async Task<List<CompanyViewModel>> GetSearchCompanyApproval(string userId, CompanySearchViewModel model)
        {
            try
            {
                string query = @"SELECT CO.[CompanyCode]
                                ,CO.[CompanyName]
                                ,CO.[CompanyType]
                                ,CO.[CompanyTypeDescr]
                                ,CO.[CategoryCode]
                                ,CO.[CompanyCategoryDesc]
                                ,CO.[LobCode]
                                ,CO.[CompanyLobDescr]
                                ,CO.[CompanyAddress]
                                ,CO.[CompanyAddress2]
                                ,CO.[KotaMadya]
                                ,CO.[Propinsi]
                                ,CO.[PropinsiDesc]
                                ,CO.[ZipCode]
                                ,CO.[Phone]
                                ,CO.[Fax]
                                ,CO.[Email]
                                ,CO.[Pic]
                                ,CO.[Pic2]
                                ,CO.[PicTitle]
                                ,CO.[RegisterDate]
                                ,CO.[Npwp]
                                ,CO.[StatusCode]
                                ,CO.[StatusDesc]
                                ,CO.[AgentCode]
                                ,CO.[AgentName]
                                ,CO.[ChanelCode]
                                ,CO.[ChannelDesc]
                                ,CO.[SubChannelCode]
                                ,CO.[SubChannelDistDesc]
                                ,CO.[AgentStartDate]
                                FROM [dbo].[V_CompanyRegistration] CO
                                WHERE CO.CreatedBy = @UserId
                                AND Isnull(CO.CompanyCode,'||') = ISNULL(NULLIF(@CompanyCode,''),Isnull(CO.CompanyCode,'||'))
                                AND Isnull(CO.StatusCode,'||') = ISNULL(NULLIF(@StatusCode,''),Isnull(CO.StatusCode,'||'))
                                AND Isnull(CO.AgentCode,'||') = ISNULL(NULLIF(@AgentCode,''),Isnull(CO.AgentCode,'||'))
                                AND Isnull(CO.CategoryCode,'||') = ISNULL(NULLIF(@CategoryCode,''),Isnull(CO.CategoryCode,'||'))
                                AND Isnull(CO.LobCode,'||') = ISNULL(NULLIF(@LobCode,''),Isnull(CO.LobCode,'||'))
                                AND Isnull(CO.CompanyName,'||') = ISNULL(NULLIF(@CompanyName,''),Isnull(CO.CompanyName,'||'))
                                AND convert(date,CO.RegisterDate) >= ISNULL(NULLIF(convert(date,@FromDate),''),CO.RegisterDate) 
                                AND convert(date,CO.RegisterDate) <= ISNULL(NULLIF(convert(date,@ToDate),''),CO.RegisterDate) 
                                AND CO.StatusCode = '001'
                                ORDER BY CO.CompanyName";

                var param = new Dictionary<string, object>
                {
                    {"@UserId", userId },
                    {"@CompanyCode", model.CompanyCode },
                    {"@StatusCode", model.Status },
                    {"@AgentCode", model.AgentCode },
                    {"@CategoryCode", model.CategoryCode },
                    {"@LobCode", model.LOB },
                    {"@CompanyName", model.CompanyName },
                    {"@FromDate", model.FromDate },
                    {"@ToDate", model.ToDate },
                };

                var data = await _repository.QueryAsync<CompanyViewModel>(query, param, false);

                return data;
            }
            catch (Exception)
            {

                return new List<CompanyViewModel>();
            }
        }

        public async Task<List<CompanyViewModel>> GetCompanyAgents(string userId, string companyCode = "")
        {
            try
            {
                string query = @"SELECT [CompanyCode]
                                ,[CompanyName]
                                ,[CompanyType]
                                ,[CompanyTypeDescr]
                                ,[CategoryCode]
                                ,[CompanyCategoryDesc]
                                ,[LobCode]
                                ,[CompanyLobDescr]
                                ,[CompanyAddress]
                                ,[CompanyAddress2]
                                ,[KotaMadya]
                                ,[Propinsi]
                                ,[PropinsiDesc]
                                ,[ZipCode]
                                ,[Phone]
                                ,[Fax]
                                ,[Email]
                                ,[Pic]
                                ,[Pic2]
                                ,[PicTitle]
                                ,[RegisterDate]
                                ,[Npwp]
                                ,[StatusCode]
                                ,[StatusDesc]
                                ,[AgentCode]
                                ,[AgentName]
                                ,[ChanelCode]
                                ,[ChannelDesc]
                                ,[SubChannelCode]
                                ,[SubChannelDistDesc]
                                ,[AgentStartDate]
                                FROM [dbo].[V_CompanyRegistration] CO
                                WHERE CreatedBy = @UserId
                                AND Isnull(CO.CompanyCode,'||') = ISNULL(NULLIF(@CompanyCode,''),Isnull(CO.CompanyCode,'||'))";

                var param = new Dictionary<string, object>
                {
                    {"@UserId", userId },
                    {"@CompanyCode", companyCode }
                };

                var data = await _repository.QueryAsync<CompanyViewModel>(query, param, false);

                return data;
            }
            catch (Exception)
            {

                return new List<CompanyViewModel>();
            }
        }
    }
}
