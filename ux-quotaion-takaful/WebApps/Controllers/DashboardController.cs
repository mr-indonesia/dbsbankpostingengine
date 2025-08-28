using Apps.Core.Interfaces;
using Apps.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApps.Controllers
{
    public class DashboardController : BaseController
    {
        private string usrId = CurrentUserName();
        public DashboardController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public IActionResult Index()
        {
            if(AppsHttpContext.Current.Session.GetString("UserId") == null)
			    return RedirectToAction("Login", "Account");            
            
            if(CurrentRoleList().Contains("AE") || CurrentRoleList().Contains("NB") || CurrentRoleList().Contains("PRC"))
                return View();
            else
				return RedirectToAction("Index", "UserInfo");
			//return View();
		}

        public IActionResult Quotation() {
            return View();
        }

        public IActionResult QuotationDetails()
        {
            return View();
        }
    }
}
