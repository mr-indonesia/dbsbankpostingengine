using App.Core.Constants;
using App.Core.Interfaces;
using App.Core.Models.Authentications;
using App.Core.Models.Migrations;
using App.Core.Models.ProductGroup;
using App.Core.Models.Users;
using AutoMapper;
using DataAccess.EFCore;
using DataAccess.EFCore.Repositories;
using nusantaratech.System.Data;
using SharedKernel.Entities;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Services
{
	public class MigrationService : EFRepository, IMigrationService
	{
		private readonly IRepository _repository;
		//private readonly IMapper _mapper;
		public MigrationService(ApplicationContext context, IRepository repository) : base(context)
		{
			this._repository = repository;
		}


		public async Task<List<Migration.AgentMigration>> GetAllAgent()
		{

			try
			{
				string query = @"SELECT A.CODE,
							A.CD,
							A.CD_DESCR,
							A.SUBCD,
							A.SUBCD_DESCR,
							A.FRONT_NAME,
							A.LAST_NAME,
							A.MID_NAME,
							A.FULLNAME,
							A.DOB,
							A.POB,
							A.GENDER,
							A.JOINTDATE,
							A.PHONE,
							A.EMAIL,
							A.UPLINER,
							A.UPLINER_NAME,
							A.BRANCH_CODE,
							A.BRANCH_DESCR,
							A.ACTIVE,
							A.ACTIVE_DESCR,
							A.READ_ONLY,
							A.TRACK,
							A.TRACK_DESCR,
							A.AGENCY_CODE,
							A.AGENCY_NAME,
							A.REFERRAL_CODE,
							A.REFERRAL_NAME,
							A.MARKET_SEGMENT,
							A.MARKET_SEGMENT_DESCR,
							A.CREATEBY,
							A.AREA_CODE,
							A.AREA_NAME
							FROM [MARKETING].[dbo].[V_M_AGENTS] A
							LEFT JOIN MGRAgent B
								ON A.CODE = B.CODE
							WHERE B.CODE IS NULL AND A.CD IS NOT NULL";

				//conn.QueryString = query;
				var data = await _repository.QueryAsync<Migration.AgentMigration>(query, null, false);

				return data;
			}
			catch (Exception)
			{

				return new List<Migration.AgentMigration>();
			}
		}

		public async Task<List<MGRAgent>> ProcessAgentMigration(List<MGRAgent> entities)
		{
			string processStat = LogIds.INPROGRESS;
			try
			{
				await context.MGRAgents.AddRangeAsync(entities);
				await context.SaveChangesAsync();
				
				return entities;
			}
			catch (Exception)
			{

				return new List<MGRAgent>();
			}
		}
	}
}
