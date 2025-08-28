using Apps.Core.DTOS;
using Apps.Core.Interfaces;
using Apps.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApps.Helpers;
using X.PagedList.Extensions;

namespace WebApps.Controllers
{
    public class UserInfoController : BaseController
    {
        private readonly CompanyInfo companyInfo;
        private string usrId = CurrentUserName();
        public UserInfoController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            companyInfo = _unitOfWOrk.CompanyBranchService.FindCompanyById(CurrentDataArea()).ConfigureAwait(true).GetAwaiter().GetResult();
        }
        public async Task<IActionResult> Index(int? Page, string Search = "")
        {
            int pageSize = 10;
            int pageNumber = (Page ?? 1);
            IEnumerable<UserDTO> users = await _unitOfWOrk.UserInfoService.GetAllUser(Search);
            var pagedList = users.ToPagedList(pageNumber, pageSize);

            ViewBag.Search = Search;
            ViewBag.Notify = AppsHttpContext.Current.Session.GetString("MessageInfo") ?? string.Empty;
            AppsHttpContext.Current.Session.SetString("MessageInfo", string.Empty);

            return View(users.ToPagedList(pageNumber, pageSize));
        }

        public async Task<IActionResult> Edit(string id)
        {
            UserDTO user = await _unitOfWOrk.UserInfoService.FindUserinfoById(id);

            IEnumerable<SelectListItem> companySelected;
            companySelected = new SelectList(_unitOfWOrk.CompanyBranchService.FindDdlBranch().ConfigureAwait(true).GetAwaiter().GetResult(), "Value", "ListBoxDisplay", user.BranchCode);

            ViewBag.companySelected = companySelected;
            ViewBag.Notify = AppsHttpContext.Current.Session.GetString("MessageInfo") ?? string.Empty;
            AppsHttpContext.Current.Session.SetString("MessageInfo", string.Empty);

            return View(user);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, UserDTO model)
        {
            string message = string.Empty;
            try
            {
				//cheking user fist
				//UserDTO user = await _unitOfWOrk.UserInfoService.FindUserinfoById(id);
                if(await _unitOfWOrk.UserInfoService.UpdateUserInfo(model))
                {
					NotifyHelper.SetNotification("Edit User untuk username", model.UserCode, NotifyHelper.AllertType.Success, this);
                }
                else
                {
					NotifyHelper.SetNotification("Edir User", "Edit user failed", NotifyHelper.AllertType.Error, this);
				}

				return RedirectToAction("Index");
			}
            catch (Exception ex)
            {
				NotifyHelper.SetNotification("Edit User", ex.Message, NotifyHelper.AllertType.Error, this);
				return RedirectToAction("Edit", new { id });
			}
        }
	}
}
