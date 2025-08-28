using Apps.Core.DTOS;
using Apps.Core.Interfaces;
using Apps.Core.Models;
using Apps.Core.UnitOfWorks;
using DataAccess.EFCore.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApps.Helpers;
using X.PagedList.Extensions;

namespace WebApps.Controllers
{
    public class UserMappingController : BaseController
    {
        private readonly CompanyInfo companyInfo;
        private string usrId = CurrentUserName();
        public UserMappingController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            companyInfo = _unitOfWOrk.CompanyBranchService.FindCompanyById(CurrentDataArea()).ConfigureAwait(true).GetAwaiter().GetResult(); 
        }
        public async Task<IActionResult> Index(int? Page, string Search = "")
        {
            IEnumerable<UserDTO> users = await _unitOfWOrk.UserMappingService.GetAllUserMapping();
            int pageNumber = Page ?? 1;
            int pageSize = 10;
            var pagedList = users.ToPagedList(pageNumber, pageSize);

            //IEnumerable<SelectListItem> roleAccessList;
            //IEnumerable<SelectListItem> ddlUserList;
            //roleAccessList = new SelectList(_unitOfWOrk.ReferenceTableService.FindDdlRoleAcces().ConfigureAwait(true).GetAwaiter().GetResult(), "Value", "ListBoxDisplay", "0");
            //ddlUserList = new SelectList(_unitOfWOrk.ReferenceTableService.FindDdlUser().ConfigureAwait(true).GetAwaiter().GetResult(), "Value", "ListBoxDisplay", "0");

            //ViewBag.roleAccessList = roleAccessList;
            //ViewBag.ddlUserList = ddlUserList;

            ViewBag.Notify = AppsHttpContext.Current.Session.GetString("MessageInfo") ?? string.Empty;
            AppsHttpContext.Current.Session.SetString("MessageInfo", string.Empty);

            return View(users.ToPagedList(pageNumber, pageSize));
        }

        public async Task<IActionResult> Edit(string id)
        {
            List<CompanyUserDTO> companyUsers = await _unitOfWOrk.CompanyBranchService.FindBranchByUserId(id);
            List<BranchDTO> branches = new List<BranchDTO>();
            foreach (BranchDTO model in _unitOfWOrk.CompanyBranchService.FindAllBranch().GetAwaiter().GetResult())
            {
                bool selected = false;
                if (companyUsers.Where(a => a.BranchCode == model.BranchCode).Count() > 0)
                {
                    selected = true;
                }
                branches.Add(new BranchDTO { Selected = selected, BranchCode = model.BranchCode, BranchName = model.BranchName });
            }

            List<RoleDTO> roles = await _unitOfWOrk.RoleService.GetAllRole();
            //var isAdministrator = FindRoleApplication("ADM");
            List<RoleApplicationDTO> roleApplications = await _unitOfWOrk.RoleService.GetRoleApplicationByUserId(id);
            foreach (RoleDTO item in roles)
            {
               if(roleApplications.Where(a => a.RoleApplicationId == item.RoleAccessID).Count() > 0)
                {
                    item.Selected = true;
                }
            }

            ViewBag.UserName = id;
            ViewBag.BranchList = branches;
            ViewBag.RoleApplicationList = roles;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, IFormCollection collection)
        {
            try
            {
                string SelectedCompany = string.IsNullOrEmpty(collection["SelectedCompany"].ToString()) ? string.Empty : collection["SelectedCompany"];
                string SelectedRole = !string.IsNullOrEmpty(collection["SelectedRole"].ToString()) ? collection["SelectedRole"].ToString() : string.Empty;
                string[] SelectedRoleList = SelectedRole.Split(',');

                if (await _unitOfWOrk.RoleService.RemoveRoleByUserId(id))
                {
                    if (SelectedRoleList.Count() > 0)
                    {
                        if (SelectedRoleList[0].Length > 0)
                        {
                            if(await _unitOfWOrk.RoleService.AddRoleApplication(id, SelectedRoleList))
                            {
                                NotifyHelper.SetNotification("User Mapping Success", string.Format("For user {0}", id), NotifyHelper.AllertType.Success, this);
                            }
                            else
                            {
                                NotifyHelper.SetNotification("User Mapping failed", string.Format("For user {0}", id), NotifyHelper.AllertType.Error, this);
                            }
                        }
                            
                    }
                }
            }
            catch (Exception ex)
            {

                NotifyHelper.SetNotification("User Mapping Canceled", ex.Message, NotifyHelper.AllertType.Error, this);
            }

            return RedirectToAction("Index");
        }
    }
}
