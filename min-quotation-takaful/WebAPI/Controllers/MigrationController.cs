using App.Core.Interfaces;
using App.Core.Models.Authentications;
using App.Core.Models.Login;
using Microsoft.AspNetCore.Mvc;
using nusantaratech.System.Data;
using nusantaratech.System.Web;
using System.Threading.Tasks;
using System;
using App.Core.Models.ProductGroup;
using System.Collections.Generic;
using App.Core.Models.Migrations;
using AutoMapper;
using SharedKernel.Entities;

namespace WebAPI.Controllers
{
	public class MigrationController : BaseController
	{
		private readonly IMapper _mapper;
		public MigrationController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
		{
			_mapper = mapper;
		}


		[HttpGet("GetAllAgentMigration")]
		[ProducesResponseType(typeof(ResponseData<MGRAgent>), 200)]
		public async Task<IActionResult> GetAllAgentMigration()
		{
			try
			{
				var data = (await _unitOfWork.MigrationService.GetAllAgent());
				var agents = _mapper.Map<List<MGRAgent>>(data);
				var result = await _unitOfWork.MigrationService.ProcessAgentMigration(agents);
				HttpResults = new ResponseData<IEnumerable<MGRAgent>>("Get All Agent Migrarion", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Success, result);
			}
			catch (Exception ex)
			{

				HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);				
			}

			return HttpResponse(HttpResults);

		}
	}
}
