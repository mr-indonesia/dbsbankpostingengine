using App.Core.Interfaces;
using App.Core.UnitOfWorks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using nusantaratech.System.Data;
using nusantaratech.System.Web;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [ProducesResponseType(typeof(ResponseMessage), 500)]
    [ProducesResponseType(typeof(ResponseMessage), 400)]
    [ProducesResponseType(typeof(ResponseData<Dictionary<string, string>>), 500)]
    public class BaseController : Controller
    {
        protected HttpResult HttpResults;
        protected IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public BaseController() { }
        public BaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

		public BaseController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
            _mapper = mapper;
		}

		public ObjectResult HttpResponse(HttpResult results)
        {
            ObjectResult objectResult = new ObjectResult(results);
            objectResult.StatusCode = results.GetResponseStatusCode();
            objectResult.Value = results;

            return objectResult;
        }
    }
}
