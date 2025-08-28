using Apps.Core.DTOS;
using Apps.Core.Interfaces;
using Apps.Core.Models;
using Apps.Core.UnitOfWorks;
using DataAccess.EFCore.Entities;
using Microsoft.AspNetCore.Http;
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
    public class WorkflowController : BaseController
    {
        private readonly CompanyInfo companyInfo;
        private string usrId = CurrentUserName();
        public WorkflowController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            companyInfo = _unitOfWOrk.CompanyBranchService.FindCompanyById(CurrentDataArea()).ConfigureAwait(true).GetAwaiter().GetResult();
        }
        public async Task<IActionResult> Index(int? Page, string Search = "")
        {
            IEnumerable<WorkflowDTO> workflowDTOs = await _unitOfWOrk.WorkflowService.GetAllWorkflows();
            int pageNumber = Page ?? 1;
            int pageSize = 10;

            IEnumerable<SelectListItem> ProductGroups;
            ProductGroups = new SelectList(_unitOfWOrk.ReferenceTableService.FindDllProductGroup().ConfigureAwait(true).GetAwaiter().GetResult(), "Value", "ListBoxDisplay", "0");

            ViewBag.ProductGroups = ProductGroups;

            ViewBag.Notify = AppsHttpContext.Current.Session.GetString("MessageInfo") ?? string.Empty;
            AppsHttpContext.Current.Session.SetString("MessageInfo", string.Empty);

            return View(workflowDTOs.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddWorkflow(IFormCollection collection)
        {
            string headerId = string.Empty;
            try
            {
                string selectedGroup = string.IsNullOrEmpty(collection["ProductGroups"].ToString()) ? string.Empty : collection["ProductGroups"];
                string wfName = !string.IsNullOrEmpty(collection["WorkflowName"].ToString()) ? collection["WorkflowName"].ToString() : string.Empty;

                //setting model
                WorkflowDTO data = new WorkflowDTO();
                data.ModifiedBy = usrId;
                data.ModifiedDate = DateTime.Now;
                data.Status = false;
                data.Name = wfName;
                data.CategoryTypeID = selectedGroup;

                //process save workflow
                var result = await _unitOfWOrk.WorkflowService.AddWorkFlow(data);
                if(result != null)
                {
                    NotifyHelper.SetNotification("Add Workflow Success", string.Format("WorkflowId {0}", "WF0001"), NotifyHelper.AllertType.Success, this);
                    headerId = result.HeaderID;
                }
                else
                {
                    NotifyHelper.SetNotification("Add Workflow Failed", string.Format("WorkflowId {0}", "WF0001"), NotifyHelper.AllertType.Success, this);
                }    
            }
            catch (Exception ex)
            {
                if(ex.InnerException != null)
                {
                    NotifyHelper.SetNotification("Add Workflow Canceled", ex.InnerException.Message, NotifyHelper.AllertType.Error, this);
                }
                else
                {
                    NotifyHelper.SetNotification("Add Workflow Canceled", ex.Message, NotifyHelper.AllertType.Error, this);
                }
                
                return RedirectToAction("Index", "Workflow");
            }

            return RedirectToAction("Detail", "Workflow", new { id = headerId });
        }

        public async Task<IActionResult> Detail(string id)
        {
            var data = await _unitOfWOrk.WorkflowService.GetWorkflowById(id);
            var details = await _unitOfWOrk.WorkflowService.GetWorkflowDetailById(data.HeaderID);
            data.Details = details;

            //set nextSeq
            int nextSeq = 0;
            if(details != null && details.Count > 0)
            {
                nextSeq = details.Max(m => m.State);
                nextSeq = nextSeq + 1;
            }

            IEnumerable<SelectListItem> ProductGroups;
            IEnumerable<SelectListItem> RoleAccessList;
            IEnumerable<SelectListItem> WfAmountList;
            ProductGroups = new SelectList((await _unitOfWOrk.ReferenceTableService.FindDllProductGroup()), "Value", "ListBoxDisplay", data.CategoryTypeID);
            RoleAccessList = new SelectList((await _unitOfWOrk.ReferenceTableService.FindDdlRoleAcces()), "Value", "ListBoxDisplay", "");
            WfAmountList = new SelectList((await _unitOfWOrk.ReferenceTableService.FindDdlAmount()), "Value", "ListBoxDisplay", "");

            ViewBag.ProductGroups = ProductGroups;
            ViewBag.RoleAccessList = RoleAccessList;
            ViewBag.WfAmountList = WfAmountList;
            ViewBag.nextSeq = nextSeq;

            ViewBag.Notify = AppsHttpContext.Current.Session.GetString("MessageInfo") ?? string.Empty;
            AppsHttpContext.Current.Session.SetString("MessageInfo", string.Empty);

            return View(data);
        }

        public async Task<IActionResult> Approver(string id)
        {
            var data = await _unitOfWOrk.WorkflowService.GetWorkflowById(id);
            return View();
        }
    }
}
