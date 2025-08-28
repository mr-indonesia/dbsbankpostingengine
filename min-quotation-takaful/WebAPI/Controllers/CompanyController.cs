using App.Core.Interfaces;
using App.Core.UnitOfWorks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using App.Core.Models.Company;
using SharedKernel.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using App.Core.Models.ProductGroup;
using nusantaratech.System.Data;
using nusantaratech.System.Web;

namespace WebAPI.Controllers
{
    [Authorize]
    public class CompanyController : BaseController
    {
        private const string DateFormat = "yyyy-MM-dd";
        public static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings() { DateFormatString = DateFormat, Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore, ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor };

        private readonly IMapper _mapper;
        public CompanyController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        [HttpGet("GetCompanyAgents/{id}")]
        [ProducesResponseType(typeof(ResponseData<CompanyViewModel>), 200)]
        public async Task<IActionResult> GetCompanyAgents(string id)
        {
            try
            {
                var data = (await _unitOfWork.CompanyService.GetCompanyAgents(id));
                HttpResults = new ResponseData<IEnumerable<CompanyViewModel>>("Get All Company Agents", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Success, data);
            }
            catch (Exception ex)
            {

                HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);
            }
            return HttpResponse(HttpResults);
        }

        [HttpGet("GetSearchCompany/{id}")]
        [ProducesResponseType(typeof(ResponseData<CompanyViewModel>), 200)]
        public async Task<IActionResult> GetCompanyAgents(string id, [FromQuery] CompanySearchViewModel request)
        {
            try
            {
                var data = (await _unitOfWork.CompanyService.GetSearchCompanyAgents(id, request));
                HttpResults = new ResponseData<IEnumerable<CompanyViewModel>>("Get Search Company", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Success, data);
            }
            catch (Exception ex)
            {

                HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);
            }
            return HttpResponse(HttpResults);
        }

        [HttpGet("GetSearchCompanyApproval/{id}")]
        [ProducesResponseType(typeof(ResponseData<CompanyViewModel>), 200)]
        public async Task<IActionResult> GetSearchCompanyApproval(string id, [FromQuery] CompanySearchViewModel request)
        {
            try
            {
                var data = (await _unitOfWork.CompanyService.GetSearchCompanyApproval(id, request));
                HttpResults = new ResponseData<IEnumerable<CompanyViewModel>>("Get Search Company Approval", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Success, data);
            }
            catch (Exception ex)
            {

                HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);
            }
            return HttpResponse(HttpResults);
        }

        [HttpGet("GetCompanyAgents/{id}/{companyCode}")]
        [ProducesResponseType(typeof(ResponseData<CompanyViewModel>), 200)]
        public async Task<IActionResult> GetCompanyAgents(string id, string companyCode)
        {
            try
            {
                var data = (await _unitOfWork.CompanyService.GetCompanyAgents(id, companyCode));
                HttpResults = new ResponseData<IEnumerable<CompanyViewModel>>("Get All Company Agents by company code", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Success, data);
            }
            catch (Exception ex)
            {

                HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);
            }
            return HttpResponse(HttpResults);
        }

        [HttpPost]
        [Route("CreateCompanyAgent")]
        public async Task<IActionResult> CreateCompanyAgent([FromBody] CompanyRequest data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _unitOfWork.CompanyService.InsertCompanyAgent(data);

                    if(result.added)
                    {
                        HttpResults = new ResponseData<string>("Insert Company Agent", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Success, result.message);
                    }
                    else
                    {
                        HttpResults = new ResponseData<string>("Insert Company Agent", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Fail, result.message);
                    }
                    
                }
                catch (Exception ex)
                {
                    HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);
                }

            }

            return HttpResponse(HttpResults);
        }

		[HttpPost]
		[Route("UpdateCompanyAgent")]
		public async Task<IActionResult> UpdateCompanyAgent([FromBody] CompanyRequest data)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var result = await _unitOfWork.CompanyService.InsertCompanyAgent(data);

					if (result.added)
					{
						HttpResults = new ResponseData<string>("Update Company Agent", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Success, result.message);
					}
					else
					{
						HttpResults = new ResponseData<string>("Update Company Agent", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Fail, result.message);
					}

				}
				catch (Exception ex)
				{
					HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);
				}

			}

			return HttpResponse(HttpResults);
		}

		[HttpPost]
		[Route("ApproveCompanyAgent")]
		public async Task<IActionResult> ApproveCompanyAgent([FromBody] CompanyApproveRequest data)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var result = await _unitOfWork.CompanyService.ApproveCompanyAgent(data);

					if (result.added)
					{
						HttpResults = new ResponseData<string>("Update Company Agent", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Success, result.message);
					}
					else
					{
						HttpResults = new ResponseData<string>("Update Company Agent", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Fail, result.message);
					}

				}
				catch (Exception ex)
				{
					HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);
				}

			}

			return HttpResponse(HttpResults);
		}

        [HttpPost]
        [Route("RemoveCompanyAgent")]
        public async Task<IActionResult> RemoveCompanyAgent([FromBody] CompanyApproveRequest data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _unitOfWork.CompanyService.RemoveCompanyAgent(data);

                    if (result.added)
                    {
                        HttpResults = new ResponseData<string>("Remove Company Agent", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Success, result.message);
                    }
                    else
                    {
                        HttpResults = new ResponseData<string>("Remove Company Agent", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Fail, result.message);
                    }

                }
                catch (Exception ex)
                {
                    HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);
                }

            }

            return HttpResponse(HttpResults);
        }

        [HttpPost]
        [Route("ValidateCompanyAgent")]
        public async Task<IActionResult> ValidateCompanyAgent([FromBody] CompanyRequest data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _unitOfWork.CompanyService.ValidateCompanyAgent(data);

                    if (result.valid)
                    {
                        HttpResults = new ResponseData<string>("Validasi Company Agent", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Success, result.message);
                    }
                    else
                    {
                        HttpResults = new ResponseData<string>("Validasi Company Agent", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Fail, result.message);
                    }

                }
                catch (Exception ex)
                {
                    HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);
                }

            }

            return HttpResponse(HttpResults);
        }
    }
}
