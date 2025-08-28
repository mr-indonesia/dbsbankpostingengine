using Apps.Core.Models.Migrations;
using DataAccess.EFCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Interfaces
{
	public interface IMigrationService : IRepository
	{
		Task<List<Migration.AgentMigrationViewModel>> GetAgentMigrations();
	}
}
