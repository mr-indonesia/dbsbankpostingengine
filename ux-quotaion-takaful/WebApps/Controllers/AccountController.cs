using Apps.Core.Interfaces;
using Apps.Core.Models;
using Apps.Core.Models.Users;
using DataAccess.EFCore.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebApps.Helpers;

namespace WebApps.Controllers
{
	public class AccountController : BaseController
	{
		public AccountController(IUnitOfWork unitOfWork) : base(unitOfWork)
		{
		}

		public IActionResult Login()
		{
			ViewBag.Notify = AppsHttpContext.Current.Session.GetString("MessageInfo") ?? string.Empty;
			AppsHttpContext.Current.Session.SetString("MessageInfo", string.Empty);

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(IFormCollection collection)
		{
			try
			{
				if(ModelState.IsValid)
				{
					string usr = string.IsNullOrEmpty(collection["username"].ToString()) ? string.Empty : collection["username"];
					string pwd = !string.IsNullOrEmpty(collection["password"].ToString()) ? collection["password"].ToString() : string.Empty;
					//UserRequest user = new UserRequest { 
					//	Username = usr,
					//	Password = pwd
					//};

					var handler = new HttpClientHandler();
					handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
					{
						// For development/testing, you might always return true to bypass validation
						// In production, you would implement proper validation logic based on 'errors'
						return true;
					};

					UserModel.UserRequest req = new UserModel.UserRequest
					{
						Username = usr,
						Password = pwd
					};
					var result = await _unitOfWOrk.IdentityService.AuthecticateAsync(req);

					if (result != null && result.Success)
					{
						AppsHttpContext.Current.Session.SetString("UserId", usr);
						AppsHttpContext.Current.Session.SetString("SessTokenValidate", result.Token);
						return RedirectToAction("Index", "Dashboard");
					}
					else
					{
						AppsHttpContext.Current.Session.SetString("UserId", string.Empty);
						ViewBag.errmsg = result.ErrMessage;
						ModelState.AddModelError("", result.ErrMessage);
						return View();
					}
				}
				else
				{
					return View();
				}
				

				/*using (var client = new HttpClient(handler))
				{
					
					var url = EnumParam.ApiUrl;
					string json = JsonConvert.SerializeObject(user);
					byte[] buffer = Encoding.UTF8.GetBytes(json);
					var byteContent = new ByteArrayContent(buffer);
					byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
					var httpResponce = client.PostAsync(url, byteContent).Result;

					if (httpResponce.IsSuccessStatusCode)
					{
						var responseContent = httpResponce.Content;
						string result = responseContent.ReadAsStringAsync().Result;

						JObject finalResult = JObject.Parse(result);

						if(!string.IsNullOrEmpty(finalResult["message"].ToString()))
						{
                            AppsHttpContext.Current.Session.SetString("UserId", string.Empty);
                            ViewBag.errmsg = finalResult["message"].ToString();
							ModelState.AddModelError("", finalResult["message"].ToString());
							return View();
						}

                        AppsHttpContext.Current.Session.SetString("UserId", usr);
                        return RedirectToAction("Index", "Workflow");
					}
					else
					{
                        AppsHttpContext.Current.Session.SetString("UserId", string.Empty);
                        ViewBag.errmsg = "Something when wrong";
						ModelState.AddModelError("", "Something when wrong");
						return View();
					}
				}*/
			}
			catch (Exception ex)
			{
                AppsHttpContext.Current.Session.SetString("UserId", string.Empty);
                ViewBag.errmsg = string.Format("Internal Server Error : {0}", ex.InnerException.Message.ToString());
				ModelState.AddModelError("", string.Format("Internal Server Error : {0}", ex.InnerException.Message.ToString()));
				return View();
			}
		}
	}
}
