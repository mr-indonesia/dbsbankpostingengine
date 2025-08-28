using App.Core.Models.Authentications;
using App.Core.Models.Migrations;
using nusantaratech.System.Data;
using SharedKernel.Entities;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
	public interface IMigrationService : IRepository
	{
		Task<List<MGRAgent>> ProcessAgentMigration(List<MGRAgent> entities);
		Task<List<Migration.AgentMigration>> GetAllAgent();
	}
}
