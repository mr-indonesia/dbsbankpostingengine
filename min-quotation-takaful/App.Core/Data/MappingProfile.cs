using App.Core.Models.Company;
using App.Core.Models.Migrations;
using AutoMapper;
using AutoMapper.Configuration;
using SharedKernel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Data
{
	public class MappingProfile : Profile
	{
		public MappingProfile() {
			CreateMap<MGRAgent, Migration.AgentMigration>();
			CreateMap<Migration.AgentMigration, MGRAgent>();
			CreateMap<CompanyRequest, MstCompanyAgent>();
			CreateMap<MstCompanyAgent, CompanyRequest>();
		}
	}
}
