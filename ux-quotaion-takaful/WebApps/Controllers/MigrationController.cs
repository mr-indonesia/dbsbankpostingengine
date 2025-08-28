using Apps.Core.DTOS;
using Apps.Core.Interfaces;
using Apps.Core.Models;
using Apps.Core.Models.Migrations;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList.Extensions;

namespace WebApps.Controllers
{
	public class MigrationController : BaseController
	{
		private readonly CompanyInfo companyInfo;
		private string usrId = CurrentUserName();
		public MigrationController(IUnitOfWork unitOfWork) : base(unitOfWork)
		{
			companyInfo = _unitOfWOrk.CompanyBranchService.FindCompanyById(CurrentDataArea()).ConfigureAwait(true).GetAwaiter().GetResult();
		}
		public async Task<IActionResult> Index(int? Page, string Search = "")
		{
			IEnumerable<Migration.AgentMigrationViewModel> agents = await _unitOfWOrk.MigrationService.GetAgentMigrations();
			int pageNumber = Page ?? 1;
			int pageSize = 10;

			return View(agents.ToPagedList(pageNumber, pageSize));
		}
	}
}
