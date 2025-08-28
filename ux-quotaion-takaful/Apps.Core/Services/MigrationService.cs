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
using Apps.Core.Models.Migrations;
using Apps.Core.DTOS;
using DataAccess.EFCore.Entities;

namespace Apps.Core.Services
{
	public class MigrationService : EFRepository, IMigrationService
	{
		private readonly IRepository _repository;
		public MigrationService(ApplicationContext context, IRepository repository) : base(context)
		{
			this._repository = repository;
		}

		public async Task<List<Migration.AgentMigrationViewModel>> GetAgentMigrations()
		{
			try
			{
				string query = $@"SELECT AgentCode = AG.CODE
								  ,ChanelCode = AG.CD
								  ,ChannelDesc = AG.CD_DESCR
								  ,SubChannelCode = AG.SUBCD
								  ,SubChannelDesc = AG.SUBCD_DESCR
								  ,AG.FRONT_NAME
								  ,AG.LAST_NAME
								  ,AG.MID_NAME
								  ,AG.FULLNAME
								  ,DateOfBirth = AG.DOB
								  ,Gender = AG.GENDER
								  ,JoinDate = AG.JOINTDATE
								  ,Phone = AG.PHONE
								  ,Email = AG.EMAIL
								  ,Upliner = UPLINER
								  ,UplinerName = AG.UPLINER_NAME
							  FROM [dbo].[MGRAgent] AG";

				var data = await _repository.QueryListAsync<Migration.AgentMigrationViewModel>(query, null, false);

				return data;
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
