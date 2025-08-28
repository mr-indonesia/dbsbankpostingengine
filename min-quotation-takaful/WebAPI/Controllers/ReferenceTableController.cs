using App.Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using nusantaratech.System.Data;
using nusantaratech.System.Web;
using SharedKernel.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using App.Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [Authorize]
    public class ReferenceTableController : BaseController
    {
        private readonly IMapper _mapper;
        public ReferenceTableController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        [HttpGet("FindDllProductGroup")]
        [ProducesResponseType(typeof(ResponseData<DdlGroupProduct>), 200)]
        public async Task<IActionResult> FindDllProductGroup()
        {
            try
            {
                var data = (await _unitOfWork.ReferenceTableService.FindDllProductGroup());
                HttpResults = new ResponseData<IEnumerable<DdlGroupProduct>>("Find ddl product group", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Success, data);
            }
            catch (Exception ex)
            {

                HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);
            }

            return HttpResponse(HttpResults);

        }

        [HttpGet("FindDdlRoleAcces")]
        [ProducesResponseType(typeof(ResponseData<DdlRoleAccess>), 200)]
        public async Task<IActionResult> FindDdlRoleAcces()
        {
            try
            {
                var data = (await _unitOfWork.ReferenceTableService.FindDdlRoleAcces());
                HttpResults = new ResponseData<IEnumerable<DdlRoleAccess>>("Find ddl role access", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Success, data);
            }
            catch (Exception ex)
            {

                HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);
            }

            return HttpResponse(HttpResults);

        }

        [HttpGet("FindDdlAmount")]
        [ProducesResponseType(typeof(ResponseData<DdlAmount>), 200)]
        public async Task<IActionResult> FindDdlAmount()
        {
            try
            {
                var data = (await _unitOfWork.ReferenceTableService.FindDdlAmount());
                HttpResults = new ResponseData<IEnumerable<DdlAmount>>("Find ddl amount", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Success, data);
            }
            catch (Exception ex)
            {

                HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);
            }

            return HttpResponse(HttpResults);

        }

        [HttpGet("FindDdlUser")]
        [ProducesResponseType(typeof(ResponseData<DdlUser>), 200)]
        public async Task<IActionResult> FindDdlUser()
        {
            try
            {
                var data = (await _unitOfWork.ReferenceTableService.FindDdlUser());
                HttpResults = new ResponseData<IEnumerable<DdlUser>>("Find ddl user", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Success, data);
            }
            catch (Exception ex)
            {

                HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);
            }

            return HttpResponse(HttpResults);

        }

        [HttpGet("FindDdlCompanyType")]
        [ProducesResponseType(typeof(ResponseData<DdlCompanyType>), 200)]
        public async Task<IActionResult> FindDdlCompanyType()
        {
            try
            {
                var data = (await _unitOfWork.ReferenceTableService.FindDdlCompanyType());
                HttpResults = new ResponseData<IEnumerable<DdlCompanyType>>("Find ddl company type", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Success, data);
            }
            catch (Exception ex)
            {

                HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);
            }

            return HttpResponse(HttpResults);

        }

        [HttpGet("FindDdlCompanyLOB")]
        [ProducesResponseType(typeof(ResponseData<DdlCompanyLOB>), 200)]
        public async Task<IActionResult> FindDdlCompanyLOB()
        {
            try
            {
                var data = (await _unitOfWork.ReferenceTableService.FindDdlCompanyLOB());
                HttpResults = new ResponseData<IEnumerable<DdlCompanyLOB>>("Find ddl company lob", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Success, data);
            }
            catch (Exception ex)
            {

                HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);
            }

            return HttpResponse(HttpResults);

        }

        [HttpGet("FindDdlCompanyCategory")]
        [ProducesResponseType(typeof(ResponseData<DdlCompanyCategory>), 200)]
        public async Task<IActionResult> FindDdlCompanyCategory()
        {
            try
            {
                var data = (await _unitOfWork.ReferenceTableService.FindDdlCompanyCategory());
                HttpResults = new ResponseData<IEnumerable<DdlCompanyCategory>>("Find ddl company category", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Success, data);
            }
            catch (Exception ex)
            {

                HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);
            }

            return HttpResponse(HttpResults);

        }

        [HttpGet("FindDdlCompanyStatus")]
        [ProducesResponseType(typeof(ResponseData<DdlCompanyStatus>), 200)]
        public async Task<IActionResult> FindDdlCompanyStatus()
        {
            try
            {
                var data = (await _unitOfWork.ReferenceTableService.FindDdlCompanyStatus());
                HttpResults = new ResponseData<IEnumerable<DdlCompanyStatus>>("Find ddl company status", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Success, data);
            }
            catch (Exception ex)
            {

                HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);
            }

            return HttpResponse(HttpResults);

        }

        [HttpGet("FindDdlCompanyStatus/{code}")]
        [ProducesResponseType(typeof(ResponseData<DdlCompanyStatus>), 200)]
        public async Task<IActionResult> FindDdlCompanyStatus(string code)
        {
            try
            {
                var data = (await _unitOfWork.ReferenceTableService.FindDdlCompanyStatus(code));
                HttpResults = new ResponseData<IEnumerable<DdlCompanyStatus>>("Find ddl company status by code", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Success, data);
            }
            catch (Exception ex)
            {

                HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);
            }

            return HttpResponse(HttpResults);

        }

        [HttpGet("FindDdlPropinsi")]
        [ProducesResponseType(typeof(ResponseData<DdlPropinsi>), 200)]
        public async Task<IActionResult> FindDdlPropinsi()
        {
            try
            {
                var data = (await _unitOfWork.ReferenceTableService.FindDdlPropinsi());
                HttpResults = new ResponseData<IEnumerable<DdlPropinsi>>("Find ddl propinsi", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Success, data);
            }
            catch (Exception ex)
            {

                HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);
            }

            return HttpResponse(HttpResults);

        }
    }
}
