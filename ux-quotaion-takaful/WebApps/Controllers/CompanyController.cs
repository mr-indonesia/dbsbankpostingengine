using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Apps.Core.Interfaces;
using Apps.Core.Models;
using System.Threading.Tasks;
using Apps.Core.Models.Company;
using Apps.Core.DTOS;
using X.PagedList.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApps.Helpers;
using System.Linq;
using DataAccess.EFCore.Entities;
using WebApps.Models;
using System.Reflection.Metadata;
using System.Globalization;

namespace WebApps.Controllers
{
	public class CompanyController : BaseController
    {
        private readonly CompanyInfo companyInfo;
        private string usrId = CurrentUserName();
        public CompanyController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            companyInfo = _unitOfWOrk.CompanyBranchService.FindCompanyById(CurrentDataArea()).ConfigureAwait(true).GetAwaiter().GetResult();
        }
        public async Task<IActionResult> Index(int? Page, CompanySearchViewModel filter = null)
        {
            // Build query parameters berdasarkan filter
            var queryParams = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(filter?.CompanyCode))
                queryParams.Add("companyCode", filter.CompanyCode);

            if (!string.IsNullOrEmpty(filter? .Status))
                queryParams.Add("status", filter.Status);

            if (!string.IsNullOrEmpty(filter?.CompanyName))
                queryParams.Add("companyName", filter.CompanyName);

            if (!string.IsNullOrEmpty(filter?.AgentCode))
                queryParams.Add("agentCode", filter.AgentCode);

            if (!string.IsNullOrEmpty(filter?.CategoryCode))
                queryParams.Add("categoryCode", filter.CategoryCode);

            if (!string.IsNullOrEmpty(filter?.AgentName))
                queryParams.Add("agentName", filter.AgentName);

            if (!string.IsNullOrEmpty(filter?.LOB))
                queryParams.Add("lob", filter.LOB);

            if (!string.IsNullOrEmpty(filter?.SubChannel))
                queryParams.Add("subChannel", filter.SubChannel);

            if (!string.IsNullOrEmpty(filter?.FromDate))
                queryParams.Add("fromDate", DateTime.ParseExact(filter.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd MMM yyyy"));

            if (!string.IsNullOrEmpty(filter?.ToDate))
                queryParams.Add("toDate", DateTime.ParseExact(filter.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd MMM yyyy"));

            // Build URL dengan query parameters
            var baseUrl = $"{BaseApiurl()}GetSearchCompany/{usrId}";
            var queryString = string.Join("&", queryParams.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
            var apiUrl = queryParams.Any() ? $"{baseUrl}?{queryString}" : baseUrl;

            //var data = await _unitOfWOrk.CompanyAgentService.GetAllCompanyAgent(string.Format($"" + BaseApiurl() + "GetCompanyAgents/" + usrId));
            var data = await _unitOfWOrk.CompanyAgentService.GetAllCompanyAgent(apiUrl);

            IEnumerable<CompanyViewModel> dataCompany = data.Returns;
            int pageNumber = Page ?? 1;
            int pageSize = 10;

            IEnumerable<SelectListItem> LstCompanyType;
            var companyType = await _unitOfWOrk.ReferenceTableService.FindDdlCompanyType(string.Format($"" + BaseApiurl() + "FindDdlCompanyType"));
            LstCompanyType = new SelectList(companyType, "Value", "ListBoxDisplay", "");

			IEnumerable<SelectListItem> LstCompanyLOB;
			var companyLOB = await _unitOfWOrk.ReferenceTableService.FindDdlCompanyLOB(string.Format($"" + BaseApiurl() + "FindDdlCompanyLOB"));
			LstCompanyLOB = new SelectList(companyLOB, "Value", "ListBoxDisplay", "");

			IEnumerable<SelectListItem> LstCompanyCategory;
			var companyCategory = await _unitOfWOrk.ReferenceTableService.FindDdlCompanyCategory(string.Format($"" + BaseApiurl() + "FindDdlCompanyCategory"));
			LstCompanyCategory = new SelectList(companyCategory, "Value", "ListBoxDisplay", "");

			IEnumerable<SelectListItem> LstCompanyStatus;
			var companyStatus = await _unitOfWOrk.ReferenceTableService.FindDdlCompanyStatus(string.Format($"" + BaseApiurl() + "FindDdlCompanyStatus"));
			LstCompanyStatus = new SelectList(companyStatus, "Value", "ListBoxDisplay", "");

			IEnumerable<SelectListItem> LstPropinsi;
			var propinsi = await _unitOfWOrk.ReferenceTableService.FindDdlPropinsi(string.Format($"" + BaseApiurl() + "FindDdlPropinsi"));
			LstPropinsi = new SelectList(propinsi, "Value", "ListBoxDisplay", "");

            // Create search view model
            var viewModel = new CompanySearchViewModel
            {
                Companies = dataCompany.ToPagedList(pageNumber, pageSize),
                CompanyCode = filter.CompanyCode,
                CompanyName = filter.CompanyName,
                AgentCode = filter.AgentCode,
                AgentName = filter.AgentName,
                Status = filter.Status,
                CategoryCode = filter.CategoryCode,
                LOB = filter.LOB,
                SubChannel = filter.SubChannel,
                FromDate = filter.FromDate,
                ToDate = filter.ToDate,
                DdlStatus = LstCompanyStatus,
                DdlCategory = LstCompanyCategory,
                DdlLOB = LstCompanyLOB
            };

            ViewBag.CompanyTypeList = LstCompanyType;
            ViewBag.CompanyLobList = LstCompanyLOB;
            ViewBag.CompanyCategoryList = LstCompanyCategory;
            ViewBag.CompanyStatusList = LstCompanyStatus;
            ViewBag.PropinsiList = LstPropinsi;

			ViewBag.Notify = AppsHttpContext.Current.Session.GetString("MessageInfo") ?? string.Empty;
            AppsHttpContext.Current.Session.SetString("MessageInfo", string.Empty);

            return View(viewModel);
            //return View(dataCompany.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var data = await _unitOfWOrk.CompanyAgentService.GetAllCompanyAgent(string.Format($"" + BaseApiurl() + "GetCompanyAgents/" + usrId + "/" + id));

            if(data.Success)
            {
                var companyAgent = data.Returns.SingleOrDefault();

                IEnumerable<SelectListItem> LstCompanyType;
                var companyType = await _unitOfWOrk.ReferenceTableService.FindDdlCompanyType(string.Format($"" + BaseApiurl() + "FindDdlCompanyType"));
                LstCompanyType = new SelectList(companyType, "Value", "ListBoxDisplay", companyAgent.CompanyType);

                IEnumerable<SelectListItem> LstCompanyLOB;
                var companyLOB = await _unitOfWOrk.ReferenceTableService.FindDdlCompanyLOB(string.Format($"" + BaseApiurl() + "FindDdlCompanyLOB"));
                LstCompanyLOB = new SelectList(companyLOB, "Value", "ListBoxDisplay", companyAgent.LobCode);

                IEnumerable<SelectListItem> LstCompanyCategory;
                var companyCategory = await _unitOfWOrk.ReferenceTableService.FindDdlCompanyCategory(string.Format($"" + BaseApiurl() + "FindDdlCompanyCategory"));
                LstCompanyCategory = new SelectList(companyCategory, "Value", "ListBoxDisplay", companyAgent.CategoryCode);

                IEnumerable<SelectListItem> LstCompanyStatus;
                var companyStatus = await _unitOfWOrk.ReferenceTableService.FindDdlCompanyStatus(string.Format($"" + BaseApiurl() + "FindDdlCompanyStatus"));
                LstCompanyStatus = new SelectList(companyStatus, "Value", "ListBoxDisplay", companyAgent.StatusCode);

                IEnumerable<SelectListItem> LstPropinsi;
                var propinsi = await _unitOfWOrk.ReferenceTableService.FindDdlPropinsi(string.Format($"" + BaseApiurl() + "FindDdlPropinsi"));
                LstPropinsi = new SelectList(propinsi, "Value", "ListBoxDisplay", companyAgent.Propinsi);

                //ViewBag.CompanyTypeList = LstCompanyType;
                //ViewBag.CompanyLobList = LstCompanyLOB;
                //ViewBag.CompanyCategoryList = LstCompanyCategory;
                //ViewBag.CompanyStatusList = LstCompanyStatus;
                //ViewBag.PropinsiList = LstPropinsi;

				//return Json(companyAgent);

				return Json(new
				{
					success = true,
					company = companyAgent,
					dropdowns = new
					{
						LstCompanyType,
						LstCompanyLOB,
						LstCompanyCategory,
						LstCompanyStatus,
						LstPropinsi
					}
				});
			}

			return Json(new { success = false, message = "Data not found" });
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCompanyAgent(IFormCollection collection)
        {
            try
            {
                string CompanyName = string.IsNullOrEmpty(collection["CompanyName"].ToString()) ? string.Empty : collection["CompanyName"];
                string Phone = string.IsNullOrEmpty(collection["Phone"].ToString()) ? string.Empty : collection["Phone"];
                string CompanyType = string.IsNullOrEmpty(collection["CompanyType"].ToString()) ? string.Empty : collection["CompanyType"];
                string Fax = string.IsNullOrEmpty(collection["Fax"].ToString()) ? string.Empty : collection["Fax"];
                string Category = string.IsNullOrEmpty(collection["Category"].ToString()) ? string.Empty : collection["Category"];
                string Email = string.IsNullOrEmpty(collection["Email"].ToString()) ? string.Empty : collection["Email"];
                string LobCode = string.IsNullOrEmpty(collection["LobCode"].ToString()) ? string.Empty : collection["LobCode"];
                string PIC1 = string.IsNullOrEmpty(collection["PIC1"].ToString()) ? string.Empty : collection["PIC1"];
                string Address1 = string.IsNullOrEmpty(collection["Address1"].ToString()) ? string.Empty : collection["Address1"];
                string PIC2 = string.IsNullOrEmpty(collection["PIC2"].ToString()) ? string.Empty : collection["PIC2"];
                string Address2 = string.IsNullOrEmpty(collection["Address2"].ToString()) ? string.Empty : collection["Address2"];
                string PicTitle = string.IsNullOrEmpty(collection["PicTitle"].ToString()) ? string.Empty : collection["PicTitle"];
                string kodya = string.IsNullOrEmpty(collection["kodya"].ToString()) ? string.Empty : collection["kodya"];
                string RegisterDate = string.IsNullOrEmpty(collection["RegisterDate"].ToString()) ? string.Empty : collection["RegisterDate"];
                string Propinsi = string.IsNullOrEmpty(collection["Propinsi"].ToString()) ? string.Empty : collection["Propinsi"];
                string Npwp = string.IsNullOrEmpty(collection["Npwp"].ToString()) ? string.Empty : collection["Npwp"];
                string ZipCode = string.IsNullOrEmpty(collection["ZipCode"].ToString()) ? string.Empty : collection["ZipCode"];
                string CoStatus = string.IsNullOrEmpty(collection["CoStatus"].ToString()) ? string.Empty : collection["CoStatus"];

                CompanyRequest data = new CompanyRequest { 
                    CompanyName = CompanyName,
                    Phone = Phone,
                    CompanyType = CompanyType,
                    Fax = Fax,
                    CategoryCode = Category,
                    Email = Email,
                    LobCode = LobCode,
                    Pic = PIC1,
                    CompanyAddress = Address1,
                    Pic2 = PIC2,
                    CompanyAddress2 = Address2,
                    PicTitle = PicTitle,
                    KotaMadya = kodya,
                    Propinsi = Propinsi,
                    Npwp = Npwp,
                    ZipCode = ZipCode,
                    CreatedBy = usrId
                };

                var result = await _unitOfWOrk.CompanyAgentService.AddCompanyAgent(string.Format($"" + BaseApiurl() + "CreateCompanyAgent"), data);

                if (result.Success)
                {
                    NotifyHelper.SetNotification("Add Company Agent ", result.Returns, NotifyHelper.AllertType.Success, this);
                }
                else
                {
                    NotifyHelper.SetNotification("Add Company Agent failed", result.Returns, NotifyHelper.AllertType.Info, this);
                }
            }
            catch (Exception ex)
            {

                if (ex.InnerException != null)
                {
                    NotifyHelper.SetNotification("Add Company Agent Canceled", ex.InnerException.Message, NotifyHelper.AllertType.Error, this);
                }
                else
                {
                    NotifyHelper.SetNotification("Add Company Agent Canceled", ex.Message, NotifyHelper.AllertType.Error, this);
                }

                return RedirectToAction("Index", "Company");
            }
            return RedirectToAction("Index", "Company");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCompanyAgent(IFormCollection collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Jika validasi model gagal
                    var errorMessages = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return Json(new
                    {
                        success = false,
                        message = string.Join("<br/>", errorMessages)
                    });
                }


                string CompanyName = string.IsNullOrEmpty(collection["CompanyName"].ToString()) ? string.Empty : collection["CompanyName"];
                string Phone = string.IsNullOrEmpty(collection["Phone"].ToString()) ? string.Empty : collection["Phone"];
                string CompanyType = string.IsNullOrEmpty(collection["CompanyType"].ToString()) ? string.Empty : collection["CompanyType"];
                string Fax = string.IsNullOrEmpty(collection["Fax"].ToString()) ? string.Empty : collection["Fax"];
                string Category = string.IsNullOrEmpty(collection["Category"].ToString()) ? string.Empty : collection["Category"];
                string Email = string.IsNullOrEmpty(collection["Email"].ToString()) ? string.Empty : collection["Email"];
                string LobCode = string.IsNullOrEmpty(collection["LobCode"].ToString()) ? string.Empty : collection["LobCode"];
                string PIC1 = string.IsNullOrEmpty(collection["PIC1"].ToString()) ? string.Empty : collection["PIC1"];
                string Address1 = string.IsNullOrEmpty(collection["Address1"].ToString()) ? string.Empty : collection["Address1"];
                string PIC2 = string.IsNullOrEmpty(collection["PIC2"].ToString()) ? string.Empty : collection["PIC2"];
                string Address2 = string.IsNullOrEmpty(collection["Address2"].ToString()) ? string.Empty : collection["Address2"];
                string PicTitle = string.IsNullOrEmpty(collection["PicTitle"].ToString()) ? string.Empty : collection["PicTitle"];
                string kodya = string.IsNullOrEmpty(collection["kodya"].ToString()) ? string.Empty : collection["kodya"];
                string RegisterDate = string.IsNullOrEmpty(collection["RegisterDate"].ToString()) ? string.Empty : collection["RegisterDate"];
                string Propinsi = string.IsNullOrEmpty(collection["Propinsi"].ToString()) ? string.Empty : collection["Propinsi"];
                string Npwp = string.IsNullOrEmpty(collection["Npwp"].ToString()) ? string.Empty : collection["Npwp"];
                string ZipCode = string.IsNullOrEmpty(collection["ZipCode"].ToString()) ? string.Empty : collection["ZipCode"];
                string CoStatus = string.IsNullOrEmpty(collection["CoStatus"].ToString()) ? string.Empty : collection["CoStatus"];

                CompanyRequest data = new CompanyRequest
                {
                    CompanyName = CompanyName,
                    Phone = Phone,
                    CompanyType = CompanyType,
                    Fax = Fax,
                    CategoryCode = Category,
                    Email = Email,
                    LobCode = LobCode,
                    Pic = PIC1,
                    CompanyAddress = Address1,
                    Pic2 = PIC2,
                    CompanyAddress2 = Address2,
                    PicTitle = PicTitle,
                    KotaMadya = kodya,
                    Propinsi = Propinsi,
                    Npwp = Npwp,
                    ZipCode = ZipCode,
                    CreatedBy = usrId
                };

                var companyresult = await _unitOfWOrk.CompanyAgentService.ValidateCompanyAgent(string.Format($"" + BaseApiurl() + "ValidateCompanyAgent"), data);
                if(!companyresult.Success)
                {
                    return Json(new
                    {
                        success = false,
                        message = companyresult.Returns
                    });
                }


                var result = await _unitOfWOrk.CompanyAgentService.AddCompanyAgent(string.Format($"" + BaseApiurl() + "CreateCompanyAgent"), data);

                if (result.Success)
                {
                    NotifyHelper.SetNotification("Add Company Agent ", result.Returns, NotifyHelper.AllertType.Success, this);
                    return Json(new
                    {
                        success = true,
                        message = "Data Company berhasil disimpan!"
                    });
                }
                else
                {
                    //NotifyHelper.SetNotification("Add Company Agent failed", result.Returns, NotifyHelper.AllertType.Info, this);
                    return Json(new
                    {
                        success = false,
                        message = result.Returns
                    });
                }
            }
            catch (Exception ex)
            {

                if (ex.InnerException != null)
                {
                    NotifyHelper.SetNotification("Add Company Agent Canceled", ex.InnerException.Message, NotifyHelper.AllertType.Error, this);
                    return Json(new
                    {
                        success = false,
                        message = $"Terjadi kesalahan: {ex.InnerException.Message}"
                    });
                }
                else
                {
                    NotifyHelper.SetNotification("Add Company Agent Canceled", ex.Message, NotifyHelper.AllertType.Error, this);
                    return Json(new
                    {
                        success = false,
                        message = $"Terjadi kesalahan: {ex.Message}"
                    });
                }

            }
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateCompanyAgent(IFormCollection collection, string id)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					// Jika validasi model gagal
					var errorMessages = ModelState.Values
						.SelectMany(v => v.Errors)
						.Select(e => e.ErrorMessage)
						.ToList();

					return Json(new
					{
						success = false,
						message = string.Join("<br/>", errorMessages)
					});
				}

				string CompanyName = string.IsNullOrEmpty(collection["CompanyName"].ToString()) ? string.Empty : collection["CompanyName"];
				string Phone = string.IsNullOrEmpty(collection["Phone"].ToString()) ? string.Empty : collection["Phone"];
				string CompanyType = string.IsNullOrEmpty(collection["CompanyType"].ToString()) ? string.Empty : collection["CompanyType"];
				string Fax = string.IsNullOrEmpty(collection["Fax"].ToString()) ? string.Empty : collection["Fax"];
				string Category = string.IsNullOrEmpty(collection["Category"].ToString()) ? string.Empty : collection["Category"];
				string Email = string.IsNullOrEmpty(collection["Email"].ToString()) ? string.Empty : collection["Email"];
				string LobCode = string.IsNullOrEmpty(collection["LobCode"].ToString()) ? string.Empty : collection["LobCode"];
				string PIC1 = string.IsNullOrEmpty(collection["PIC1"].ToString()) ? string.Empty : collection["PIC1"];
				string Address1 = string.IsNullOrEmpty(collection["Address1"].ToString()) ? string.Empty : collection["Address1"];
				string PIC2 = string.IsNullOrEmpty(collection["PIC2"].ToString()) ? string.Empty : collection["PIC2"];
				string Address2 = string.IsNullOrEmpty(collection["Address2"].ToString()) ? string.Empty : collection["Address2"];
				string PicTitle = string.IsNullOrEmpty(collection["PicTitle"].ToString()) ? string.Empty : collection["PicTitle"];
				string kodya = string.IsNullOrEmpty(collection["kodya"].ToString()) ? string.Empty : collection["kodya"];
				string RegisterDate = string.IsNullOrEmpty(collection["RegisterDate"].ToString()) ? string.Empty : collection["RegisterDate"];
				string Propinsi = string.IsNullOrEmpty(collection["Propinsi"].ToString()) ? string.Empty : collection["Propinsi"];
				string Npwp = string.IsNullOrEmpty(collection["Npwp"].ToString()) ? string.Empty : collection["Npwp"];
				string ZipCode = string.IsNullOrEmpty(collection["ZipCode"].ToString()) ? string.Empty : collection["ZipCode"];
				string CoStatus = string.IsNullOrEmpty(collection["CoStatus"].ToString()) ? string.Empty : collection["CoStatus"];

				CompanyRequest data = new CompanyRequest
				{
                    CompanyCode = id,
					CompanyName = CompanyName,
					Phone = Phone,
					CompanyType = CompanyType,
					Fax = Fax,
					CategoryCode = Category,
					Email = Email,
					LobCode = LobCode,
					Pic = PIC1,
					CompanyAddress = Address1,
					Pic2 = PIC2,
					CompanyAddress2 = Address2,
					PicTitle = PicTitle,
					KotaMadya = kodya,
					Propinsi = Propinsi,
					Npwp = Npwp,
					ZipCode = ZipCode,
					CreatedBy = usrId
				};

				var result = await _unitOfWOrk.CompanyAgentService.AddCompanyAgent(string.Format($"" + BaseApiurl() + "UpdateCompanyAgent"), data);

				if (result.Success)
				{
					NotifyHelper.SetNotification("Update Company Agent ", result.Returns, NotifyHelper.AllertType.Success, this);
					return Json(new
					{
						success = true,
						message = "Data Company berhasil di update!"
					});
				}
				else
				{
					//NotifyHelper.SetNotification("Add Company Agent failed", result.Returns, NotifyHelper.AllertType.Info, this);
					return Json(new
					{
						success = false,
						message = result.Returns
					});
				}
			}
			catch (Exception ex)
			{

				if (ex.InnerException != null)
				{
					NotifyHelper.SetNotification("Update Company Agent Canceled", ex.InnerException.Message, NotifyHelper.AllertType.Error, this);
					return Json(new
					{
						success = false,
						message = $"Terjadi kesalahan: {ex.InnerException.Message}"
					});
				}
				else
				{
					NotifyHelper.SetNotification("Update Company Agent Canceled", ex.Message, NotifyHelper.AllertType.Error, this);
					return Json(new
					{
						success = false,
						message = $"Terjadi kesalahan: {ex.Message}"
					});
				}
			}
		}

        public async Task<IActionResult> Approval(int? Page, CompanySearchViewModel filter = null)
        {
            // Build query parameters berdasarkan filter
            var queryParams = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(filter?.CompanyCode))
                queryParams.Add("companyCode", filter.CompanyCode);

            if (!string.IsNullOrEmpty(filter?.Status))
                queryParams.Add("status", filter.Status);

            if (!string.IsNullOrEmpty(filter?.CompanyName))
                queryParams.Add("companyName", filter.CompanyName);

            if (!string.IsNullOrEmpty(filter?.AgentCode))
                queryParams.Add("agentCode", filter.AgentCode);

            if (!string.IsNullOrEmpty(filter?.CategoryCode))
                queryParams.Add("categoryCode", filter.CategoryCode);

            if (!string.IsNullOrEmpty(filter?.AgentName))
                queryParams.Add("agentName", filter.AgentName);

            if (!string.IsNullOrEmpty(filter?.LOB))
                queryParams.Add("lob", filter.LOB);

            if (!string.IsNullOrEmpty(filter?.SubChannel))
                queryParams.Add("subChannel", filter.SubChannel);

            if (!string.IsNullOrEmpty(filter?.FromDate))
                queryParams.Add("fromDate", DateTime.ParseExact(filter.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd MMM yyyy"));

            if (!string.IsNullOrEmpty(filter?.ToDate))
                queryParams.Add("toDate", DateTime.ParseExact(filter.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd MMM yyyy"));

            // Build URL dengan query parameters
            var baseUrl = $"{BaseApiurl()}GetSearchCompanyApproval/{usrId}";
            var queryString = string.Join("&", queryParams.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
            var apiUrl = queryParams.Any() ? $"{baseUrl}?{queryString}" : baseUrl;

            //var data = await _unitOfWOrk.CompanyAgentService.GetAllCompanyAgent(string.Format($"" + BaseApiurl() + "GetCompanyAgents/" + usrId));
            var data = await _unitOfWOrk.CompanyAgentService.GetAllCompanyAgent(apiUrl);

            IEnumerable<CompanyViewModel> dataCompany = data.Returns;
            int pageNumber = Page ?? 1;
            int pageSize = 10;

            IEnumerable<SelectListItem> LstCompanyType;
            var companyType = await _unitOfWOrk.ReferenceTableService.FindDdlCompanyType(string.Format($"" + BaseApiurl() + "FindDdlCompanyType"));
            LstCompanyType = new SelectList(companyType, "Value", "ListBoxDisplay", "");

            IEnumerable<SelectListItem> LstCompanyLOB;
            var companyLOB = await _unitOfWOrk.ReferenceTableService.FindDdlCompanyLOB(string.Format($"" + BaseApiurl() + "FindDdlCompanyLOB"));
            LstCompanyLOB = new SelectList(companyLOB, "Value", "ListBoxDisplay", "");

            IEnumerable<SelectListItem> LstCompanyCategory;
            var companyCategory = await _unitOfWOrk.ReferenceTableService.FindDdlCompanyCategory(string.Format($"" + BaseApiurl() + "FindDdlCompanyCategory"));
            LstCompanyCategory = new SelectList(companyCategory, "Value", "ListBoxDisplay", "");

            IEnumerable<SelectListItem> LstCompanyStatus;
            var companyStatus = await _unitOfWOrk.ReferenceTableService.FindDdlCompanyStatus(string.Format($"" + BaseApiurl() + "FindDdlCompanyStatus/001"));
            LstCompanyStatus = new SelectList(companyStatus, "Value", "ListBoxDisplay", "");

            IEnumerable<SelectListItem> LstPropinsi;
            var propinsi = await _unitOfWOrk.ReferenceTableService.FindDdlPropinsi(string.Format($"" + BaseApiurl() + "FindDdlPropinsi"));
            LstPropinsi = new SelectList(propinsi, "Value", "ListBoxDisplay", "");

            // Create search view model
            var viewModel = new CompanySearchViewModel
            {
                Companies = dataCompany.ToPagedList(pageNumber, pageSize),
                CompanyCode = filter.CompanyCode,
                CompanyName = filter.CompanyName,
                AgentCode = filter.AgentCode,
                AgentName = filter.AgentName,
                Status = filter.Status,
                CategoryCode = filter.CategoryCode,
                LOB = filter.LOB,
                SubChannel = filter.SubChannel,
                FromDate = filter.FromDate,
                ToDate = filter.ToDate,
                DdlStatus = LstCompanyStatus,
                DdlCategory = LstCompanyCategory,
                DdlLOB = LstCompanyLOB
            };

            ViewBag.CompanyTypeList = LstCompanyType;
            ViewBag.CompanyLobList = LstCompanyLOB;
            ViewBag.CompanyCategoryList = LstCompanyCategory;
            ViewBag.CompanyStatusList = LstCompanyStatus;
            ViewBag.PropinsiList = LstPropinsi;

            ViewBag.Notify = AppsHttpContext.Current.Session.GetString("MessageInfo") ?? string.Empty;
            AppsHttpContext.Current.Session.SetString("MessageInfo", string.Empty);

            return View(viewModel);
            //return View(dataCompany.ToPagedList(pageNumber, pageSize));
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ApproveCompanyAgent(IFormCollection collection, string id)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					// Jika validasi model gagal
					var errorMessages = ModelState.Values
						.SelectMany(v => v.Errors)
						.Select(e => e.ErrorMessage)
						.ToList();

					return Json(new
					{
						success = false,
						message = string.Join("<br/>", errorMessages)
					});
				}

				//string id = string.IsNullOrEmpty(collection["Id"].ToString()) ? string.Empty : collection["Id"];

				CompanyApproveRequest data = new CompanyApproveRequest {
					CompanyCode = id,
                    Remark = string.Empty,
                    StatusCode = "002",
                    UserId = usrId
                };

                var result = await _unitOfWOrk.CompanyAgentService.ApproveCompanyAgent(string.Format($"" + BaseApiurl() + "ApproveCompanyAgent"), data);

                if (result.Success)
                {
                    NotifyHelper.SetNotification("Approve Company Agent ", result.Returns, NotifyHelper.AllertType.Success, this);
                    return Json(new
                    {
                        success = true,
                        message = "Data Company berhasil di update!"
                    });
                }
                else
                {
                    //NotifyHelper.SetNotification("Add Company Agent failed", result.Returns, NotifyHelper.AllertType.Info, this);
                    return Json(new
                    {
                        success = false,
                        message = result.Returns
                    });
                }
            }
			catch (Exception ex)
			{

                if (ex.InnerException != null)
                {
                    NotifyHelper.SetNotification("Approve Company Agent Canceled", ex.InnerException.Message, NotifyHelper.AllertType.Error, this);
                    return Json(new
                    {
                        success = false,
                        message = $"Terjadi kesalahan: {ex.InnerException.Message}"
                    });
                }
                else
                {
                    NotifyHelper.SetNotification("Approve Company Agent Canceled", ex.Message, NotifyHelper.AllertType.Error, this);
                    return Json(new
                    {
                        success = false,
                        message = $"Terjadi kesalahan: {ex.Message}"
                    });
                }
            }
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectCompanyAgent(IFormCollection collection, string id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Jika validasi model gagal
                    var errorMessages = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return Json(new
                    {
                        success = false,
                        message = string.Join("<br/>", errorMessages)
                    });
                }

                string rejectremark = string.IsNullOrEmpty(collection["rejectremark"].ToString()) ? string.Empty : collection["rejectremark"];

                CompanyApproveRequest data = new CompanyApproveRequest
                {
                    CompanyCode = id,
                    Remark = rejectremark,
                    StatusCode = "003",
                    UserId = usrId
                };

                var result = await _unitOfWOrk.CompanyAgentService.ApproveCompanyAgent(string.Format($"" + BaseApiurl() + "ApproveCompanyAgent"), data);

                if (result.Success)
                {
                    NotifyHelper.SetNotification("Reject Company Agent ", result.Returns, NotifyHelper.AllertType.Success, this);
                    return Json(new
                    {
                        success = true,
                        message = "Data Company berhasil di update!"
                    });
                }
                else
                {
                    //NotifyHelper.SetNotification("Add Company Agent failed", result.Returns, NotifyHelper.AllertType.Info, this);
                    return Json(new
                    {
                        success = false,
                        message = result.Returns
                    });
                }
            }
            catch (Exception ex)
            {

                if (ex.InnerException != null)
                {
                    NotifyHelper.SetNotification("Reject Company Agent Canceled", ex.InnerException.Message, NotifyHelper.AllertType.Error, this);
                    return Json(new
                    {
                        success = false,
                        message = $"Terjadi kesalahan: {ex.InnerException.Message}"
                    });
                }
                else
                {
                    NotifyHelper.SetNotification("Reject Company Agent Canceled", ex.Message, NotifyHelper.AllertType.Error, this);
                    return Json(new
                    {
                        success = false,
                        message = $"Terjadi kesalahan: {ex.Message}"
                    });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Jika validasi model gagal
                    var errorMessages = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return Json(new
                    {
                        success = false,
                        message = string.Join("<br/>", errorMessages)
                    });
                }

                CompanyApproveRequest data = new CompanyApproveRequest
                {
                    CompanyCode = id,
                    Remark = string.Empty,
                    StatusCode = string.Empty,
                    UserId = usrId
                };

                var result = await _unitOfWOrk.CompanyAgentService.ApproveCompanyAgent(string.Format($"" + BaseApiurl() + "RemoveCompanyAgent"), data);

                if (result.Success)
                {
                    NotifyHelper.SetNotification("Remove Company Agent ", result.Returns, NotifyHelper.AllertType.Success, this);
                }
                else
                {
                    NotifyHelper.SetNotification("Remove Company Agent failed", result.Returns, NotifyHelper.AllertType.Info, this);
                }
            }
            catch (Exception ex)
            {

                if (ex.InnerException != null)
                {
                    NotifyHelper.SetNotification("Remove Company Agent Canceled", ex.InnerException.Message, NotifyHelper.AllertType.Error, this);
                }
                else
                {
                    NotifyHelper.SetNotification("Remove Company Agent Canceled", ex.Message, NotifyHelper.AllertType.Error, this);
                }
            }

            return RedirectToAction("Index", "Company");
        }
    }
}
