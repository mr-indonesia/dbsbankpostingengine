using Apps.Core.Interfaces;
using Apps.Core.Services;
using DataAccess.EFCore.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;

namespace WebApps.Controllers
{
    public class BaseController : Controller
    {
        protected IUnitOfWork _unitOfWOrk;
        public BaseController() { }
        public BaseController(IUnitOfWork unitOfWOrk)
        {
            _unitOfWOrk = unitOfWOrk;
        }

        public string CurrentDataArea()
        {
            string dataAreaId = string.Empty;
            try
            {
                
                var userId = AppsHttpContext.Current.Session.GetString("UserId");
                if (!string.IsNullOrEmpty(userId))
                {
                    var data = _unitOfWOrk.UserInfoService.FindUserinfoById(userId).ConfigureAwait(true).GetAwaiter().GetResult();
                    if (data != null)
                    {
                        dataAreaId = data.BranchCode;
                    }
                }
                //var httpcontext = accessor.HttpContext;
                AppsHttpContext.Current.Session.SetString("DataAreaId", dataAreaId);
                //AppsHttpContext.Session.SetString("DataAreaId", "0000"); FindUserinfoById
                var dataareaid = AppsHttpContext.Current.Session.GetString("DataAreaId");
                return dataareaid;
            }
            catch (Exception)
            {
                AppsHttpContext.Current.Session.SetString("DataAreaId", "DAT");
                return "DAT";
            }
            
        }

        public string BaseApiurl()
        {
            try
            {
                var result = _unitOfWOrk.SystemConfigService.FindValueSysConfig("URL", "APIURL", "BASEURL").ConfigureAwait(true).GetAwaiter().GetResult();
                return result;
            }
            catch (Exception)
            {

                return "https://quotationservicedev.com:446/";
            }
            
        }

        public string PassportUrl()
        {
            string result = string.Empty;
            try
            {
                result = _unitOfWOrk.SystemConfigService.FindValueSysConfig("URL", "APIURL", "PASSPORTURL").ConfigureAwait(true).GetAwaiter().GetResult();
                if(string.IsNullOrEmpty(result))
                {
                    result = "https://quotationservicedev.com:446/AuthenticateUser";
                }
                
            }
            catch (Exception)
            {

                return "https://quotationservicedev.com:446/AuthenticateUser";
            }
            return result;

        }

        public static string CurrentRoleApplication()
        {			
			return "Administrator";
        }

        public static bool FindRoleApplication(string roleid)
        {
			try
			{
				var PassportokenHandler = new JwtSecurityTokenHandler();
				JwtSecurityToken passportToken = null;
				var SessTokenValidate = AppsHttpContext.Current.Session.GetString("SessTokenValidate");
				passportToken = PassportokenHandler.ReadJwtToken(SessTokenValidate.Replace(Convert.ToChar(34).ToString(), string.Empty));
				var Roles = passportToken.Claims.ToList().Where(c => c.Type == "role").Select(d => d.Value).ToList();
				foreach (var role in Roles)
				{
					if (role.ToUpper() == roleid.ToUpper())
					{
						return true;
					}
				}
			}
			catch (Exception)
			{

				return false;
			}
			return false;
		}

        public static List<string> CurrentRoleList()
        {
			try
			{
				var PassportokenHandler = new JwtSecurityTokenHandler();
				JwtSecurityToken passportToken = null;
				var SessTokenValidate = AppsHttpContext.Current.Session.GetString("SessTokenValidate");
				passportToken = PassportokenHandler.ReadJwtToken(SessTokenValidate.Replace(Convert.ToChar(34).ToString(), string.Empty));
				var Roles = passportToken.Claims.ToList().Where(c => c.Type == "role").Select(d => d.Value).ToList();
				return Roles;
			}
			catch (Exception)
			{

				return null;
			}
		}

		public static string CurrentFullName()
		{
			try
			{
				var PassportokenHandler = new JwtSecurityTokenHandler();
				JwtSecurityToken passportToken = null;
				var SessTokenValidate = AppsHttpContext.Current.Session.GetString("SessTokenValidate");
				passportToken = PassportokenHandler.ReadJwtToken(SessTokenValidate.Replace(Convert.ToChar(34).ToString(), string.Empty));
				var fullName = passportToken.Claims.FirstOrDefault(c => c.Type == "FullName").Value.ToString();
                return fullName;
			}
			catch (Exception)
			{

				return "John.Doe";
			}
		}

		public static string CurrentUserName()
        {

            try
            {
                //string usr = "johan";
                //AppsHttpContext.Current.Session.SetString("UserId", usr);
                var userId = AppsHttpContext.Current.Session.GetString("UserId");
                return userId;
            }
            catch (Exception)
            {
                AppsHttpContext.Current.Session.SetString("UserId", string.Empty);
                return string.Empty;
            }
        }
    }
}
