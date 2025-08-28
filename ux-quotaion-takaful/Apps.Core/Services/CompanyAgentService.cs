using Apps.Core.Interfaces;
using Apps.Core.Models.Authentication;
using DataAccess.EFCore.Interfaces;
using DataAccess.EFCore;
using DataAccess.EFCore.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Apps.Core.Models.Company;
using System.Net.Http.Headers;

namespace Apps.Core.Services
{
    public class CompanyAgentService : EFRepository, ICompanyAgentService
    {
        private readonly IRepository _repository;
        private readonly IApiService _httpClientService;

        public CompanyAgentService(ApplicationContext context, IRepository repository, IApiService httpClientService) : base(context)
        {
            this._repository = repository;
            _httpClientService = httpClientService;
        }

        public async Task<(bool Success, List<CompanyViewModel> Returns)> GetAllCompanyAgent(string url)
        {
            try
            {
                var httpResponse = await _httpClientService.GetAsync(url);

                if (httpResponse.Success)
                {
                    JObject finalResult = JObject.Parse(httpResponse.Returns);
                    //_httpClientService.SetToken(finalResult["data"]["token"].ToString());
                    var data = JsonConvert.DeserializeObject<List<CompanyViewModel>>(finalResult["Data"].ToString());
                    return (true, data);
                }
                else
                {
                    return (false, new List<CompanyViewModel>());
                }
            }
            catch (Exception)
            {

                return (false, new List<CompanyViewModel>());
            }
        }

        public async Task<(bool Success, string Returns)> ApproveCompanyAgent(string url, CompanyApproveRequest model)
        {
            try
            {
                string json = JsonConvert.SerializeObject(model);
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var httpResponse = await _httpClientService.PostAsync(url, byteContent);

                if (httpResponse.Success)
                {
                    JObject finalResult = JObject.Parse(httpResponse.Returns);

                    if (finalResult["Status"].ToString().ToUpper() == "FAIL")
                    {
                        return (false, finalResult["Data"].ToString());
                    }

                    return (true, finalResult["Data"].ToString());
                }

                return (false, httpResponse.Returns);
            }
            catch (Exception ex)
            {

                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string Returns)> AddCompanyAgent(string url, CompanyRequest model)
        {
            try
            {
                string json = JsonConvert.SerializeObject(model);
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var httpResponse = await _httpClientService.PostAsync(url, byteContent);

                if (httpResponse.Success)
                {
                    JObject finalResult = JObject.Parse(httpResponse.Returns);

                    if(finalResult["Status"].ToString().ToUpper() == "FAIL")
                    {
                        return (false, finalResult["Data"].ToString());
                    }

                    return (true, finalResult["Data"].ToString());
                }

                return (false, httpResponse.Returns);
            }
            catch (Exception ex)
            {

                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string Returns)> ValidateCompanyAgent(string url, CompanyRequest model)
        {
            try
            {
                string json = JsonConvert.SerializeObject(model);
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var httpResponse = await _httpClientService.PostAsync(url, byteContent);

                if (httpResponse.Success)
                {
                    JObject finalResult = JObject.Parse(httpResponse.Returns);

                    if (finalResult["Status"].ToString().ToUpper() == "FAIL")
                    {
                        return (false, finalResult["Data"].ToString());
                    }

                    return (true, finalResult["Data"].ToString());
                }

                return (false, httpResponse.Returns);
            }
            catch (Exception ex)
            {

                return (false, ex.Message);
            }
        }
    }
}
