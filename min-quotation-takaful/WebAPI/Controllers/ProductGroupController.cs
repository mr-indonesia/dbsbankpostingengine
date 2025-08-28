using App.Core.Models.ProductGroup;
using Microsoft.AspNetCore.Mvc;
using nusantaratech.System.Data;
using nusantaratech.System.Web;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using App.Core.Interfaces;
using static App.Core.Models.CompanyModel;

namespace WebAPI.Controllers
{
    public class ProductGroupController : BaseController
    {
        public ProductGroupController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        [HttpGet("GetAllProductGroup")]
        [ProducesResponseType(typeof(ResponseData<ProductGroup>), 200)]
        public async Task<IActionResult> GetAllProductGroup()
        {
            try
            {
                var data = (await _unitOfWork.ProductGroupService.GetAll());
                HttpResults = new ResponseData<IEnumerable<ProductGroup>>("Get All Product Group", nusantaratech.System.Web.StatusCode.OK, StatusMessage.Success, data);
            }
            catch (Exception ex)
            {

                HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);
            }
            return HttpResponse(HttpResults);
        }
    }
}
