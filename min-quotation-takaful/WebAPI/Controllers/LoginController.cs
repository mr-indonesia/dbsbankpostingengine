using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Core.Interfaces;
using App.Core.Models.Authentications;
using App.Core.Models.Login;
using App.Core.Models.ProductGroup;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using nusantaratech.System.Data;
using nusantaratech.System.Web;

namespace WebAPI.Controllers
{
	/*[Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase*/
    public class LoginController : BaseController
    {
        //private readonly IUnitOfWork _unitOfWork;
        //public LoginController(IUnitOfWork unitOfWork)
        //{
        //    _unitOfWork = unitOfWork;
        //}

		public LoginController(IUnitOfWork unitOfWork) : base(unitOfWork)
		{
		}

		[Route("AuthenticateUser")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel loginModel)
        {
            var data = await _unitOfWork.IdentityService.Login(loginModel.Username, loginModel.Password);
            return Ok(data);
        }

		//[HttpPost("LoginUser")]
		//[ProducesResponseType(typeof(ResponseData<TokenModel>), 200)]
		//public async Task<IActionResult> LoginUser([FromBody] LoginModel loginModel)
		//{
		//	try
		//	{
		//		var data = (await _unitOfWork.IdentityService.LoginUser(loginModel.Username, loginModel.Password));
		//		HttpResults = data;
		//	}
		//	catch (Exception ex)
		//	{

		//		HttpResults = new ResponseMessage(nusantaratech.System.Web.StatusCode.InternalServerErrorException, StatusMessage.Error, ex.Message, 0);
		//	}
		//	return HttpResponse(HttpResults);
		//}
	}
}
