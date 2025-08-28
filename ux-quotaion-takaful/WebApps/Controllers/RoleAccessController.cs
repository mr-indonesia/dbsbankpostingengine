using Apps.Core.DTOS;
using Apps.Core.Interfaces;
using Apps.Core.Models;
using Apps.Core.Models.RoleAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApps.Helpers;
using X.PagedList.Extensions;

namespace WebApps.Controllers
{
	public class RoleAccessController : BaseController
	{
		private string usrId = CurrentUserName();
		public RoleAccessController(IUnitOfWork unitOfWork) : base(unitOfWork)
		{
		}
		public async Task<IActionResult> Index(int? Page, string Search = "")
		{
			int pageSize = 10;
			int pageNumber = (Page ?? 1);
			IEnumerable<RoleAccessViewModel> users = await _unitOfWOrk.RoleAccessService.GetAllRoleAccess(Search);
			var pagedList = users.ToPagedList(pageNumber, pageSize);

			ViewBag.Search = Search;
			ViewBag.Notify = AppsHttpContext.Current.Session.GetString("MessageInfo") ?? string.Empty;
			AppsHttpContext.Current.Session.SetString("MessageInfo", string.Empty);

			return View(users.ToPagedList(pageNumber, pageSize));
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoleAccess(IFormCollection collection)
        {
            try
            {
                string roleAccessId = string.IsNullOrEmpty(collection["RoleAccessId"].ToString()) ? string.Empty : collection["RoleAccessId"];
                string roleAccessName = !string.IsNullOrEmpty(collection["RoleAccessName"].ToString()) ? collection["RoleAccessName"].ToString() : string.Empty;

                //setting model
                RoleAccessViewModel data = new RoleAccessViewModel
                {
                    RoleAccessID = roleAccessId,
                    RoleAccessName = roleAccessName,
                };

                //process save workflow
                var result = await _unitOfWOrk.RoleAccessService.CreateRoleAccess(data, usrId);
                if (result != null)
                {
                    NotifyHelper.SetNotification("Add Role Access Success", string.Format("Role Code {0}", result.RoleAccessID), NotifyHelper.AllertType.Success, this);
                }
                else
                {
                    NotifyHelper.SetNotification("Add Role Access Failed", string.Format("Role Code {0}", data.RoleAccessID), NotifyHelper.AllertType.Warning, this);
                }
            }
            catch (Exception ex)
            {

                NotifyHelper.SetNotification("Add Role Access Canceled", ex.Message, NotifyHelper.AllertType.Error, this);
            }

            return RedirectToAction("Index", "RoleAccess");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var roleAccess = await _unitOfWOrk.RoleAccessService.FindRoleAccess(id);
            if (roleAccess == null)
            {
                return NotFound();
            }
            return Json(roleAccess);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRoleAccess(IFormCollection collection, string id)
        {
            try
            {
                string roleAccessId = string.IsNullOrEmpty(collection["RoleAccessId"].ToString()) ? string.Empty : collection["RoleAccessId"];
                string roleAccessName = !string.IsNullOrEmpty(collection["RoleAccessName"].ToString()) ? collection["RoleAccessName"].ToString() : string.Empty;

                var roleAccess = await _unitOfWOrk.RoleAccessService.FindRoleAccess(id);

                if(roleAccess != null)
                {
                    roleAccess.RoleAccessName = roleAccessName;
                    roleAccess.ModifiedAt = DateTime.Now;
                    roleAccess.ModifiedBy = usrId;
                }

                //process save workflow
                var result = await _unitOfWOrk.RoleAccessService.UpdateRoleAccess(roleAccess);
                if (result != null)
                {
                    NotifyHelper.SetNotification("Update Role Access Success", string.Format("Role Code {0}", result.RoleAccessID), NotifyHelper.AllertType.Success, this);
                }
            }
            catch (Exception ex)
            {

                NotifyHelper.SetNotification("Update Role Access Canceled", ex.Message, NotifyHelper.AllertType.Error, this);
            }

            return RedirectToAction("Index", "RoleAccess");
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(string id)
        {
			try
			{
				var roleAccess = await _unitOfWOrk.RoleAccessService.FindRoleAccess(id);

				if (roleAccess != null)
				{
					if(await _unitOfWOrk.RoleAccessService.DeleteRoleAccess(roleAccess))
                    {
						NotifyHelper.SetNotification("Delete Role Access Success", string.Format("Role Code {0}", roleAccess.RoleAccessID), NotifyHelper.AllertType.Success, this);
                    }
                    else
                    {
						NotifyHelper.SetNotification("Delete Role Access Failed", string.Format("Role Code {0}", roleAccess.RoleAccessID), NotifyHelper.AllertType.Info, this);
					}
                }
                else
                {
					NotifyHelper.SetNotification("Role Access Not Found", string.Format("Role Code {0}", roleAccess.RoleAccessID), NotifyHelper.AllertType.Info, this);
				}
			}
			catch (Exception ex)
			{

				NotifyHelper.SetNotification("Delete Role Access Canceled", ex.Message, NotifyHelper.AllertType.Error, this);
			}

			return RedirectToAction("Index", "RoleAccess");
		}
	}

}
