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
using Apps.Core.Models.Users;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Apps.Core.Models.RoleAccess;
using System.Runtime.InteropServices;
using System.Security.Claims;
using DataAccess.EFCore.Entities;
using Microsoft.AspNetCore.Http;

namespace Apps.Core.Services
{
	public class IdentityService : EFRepository, IIdentityService
	{
		private readonly IRepository _repository;
		private readonly ServiceConfiguration _serviceConfiguration;
		private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IApiService _httpClientService;

        public IdentityService(ApplicationContext context, IRepository repository, IOptions<ServiceConfiguration> serviceConfiguration, TokenValidationParameters tokenValidationParameters) : base(context)
		{
			this._repository = repository;
			_serviceConfiguration = serviceConfiguration.Value;
			_tokenValidationParameters = tokenValidationParameters;
		}

        public IdentityService(ApplicationContext context, IRepository repository, IOptions<ServiceConfiguration> serviceConfiguration, TokenValidationParameters tokenValidationParameters, IApiService httpClientService) : base(context)
        {
            this._repository = repository;
            _serviceConfiguration = serviceConfiguration.Value;
            _tokenValidationParameters = tokenValidationParameters;
            _httpClientService = httpClientService;
        }

        public async Task<AuthenticationResult> AuthecticateAsync(UserModel.UserRequest userReq)
		{
			//define authetication result
			AuthenticationResult result = new AuthenticationResult();

			//define token handler
			var tokenHandler = new JwtSecurityTokenHandler();
			var PassportokenHandler = new JwtSecurityTokenHandler();
			JwtSecurityToken passportToken = null;

			try
			{				
				var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_serviceConfiguration.JwtSettings.Secret));
				var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);				

				//get token from passport
				List<string> Roles = new List<string>();
				string userName = string.Empty;
				string roleCode = string.Empty;
				string fullName = string.Empty;
				string email = string.Empty;

				string ApiUrl = _serviceConfiguration.Server.PassportUrl;
				var (issuccess, resultjwtPassport) = await AuthenticationPasspost2(ApiUrl, userReq);
				if (issuccess)
				{

					passportToken = PassportokenHandler.ReadJwtToken(resultjwtPassport.Replace(Convert.ToChar(34).ToString(), string.Empty));
				}
				else { 
				
					result.Success = false;
					result.ErrMessage = resultjwtPassport;
					return result;
				}

				//prepare for set claim identity
				try
				{
					Roles = passportToken.Claims.ToList().Where(c => c.Type == "role").Select(d => d.Value).ToList();
					userName = passportToken.Claims.FirstOrDefault(c => c.Type == "Username").Value.ToString();
					roleCode = passportToken.Claims.FirstOrDefault(c => c.Type == "RoleCode").Value.ToString();
					fullName = passportToken.Claims.FirstOrDefault(c => c.Type == "FullName").Value.ToString();
					email = passportToken.Claims.FirstOrDefault(c => c.Type == "Email").Value.ToString();					
				}
				catch (Exception ex)
				{
					result.Success = false;
					result.ErrMessage = "Token Not Authorized : " + ex.Message;
					return result;
				}

				//get roles by userid
				var UserRole = await GetRoles(userReq);
				var _ListUserROle = UserRole.Select(s => s.RoleCode).ToList();
				if (Roles.Any(a => _ListUserROle.Contains(a)))
				{

				}
				else
				{
					result.Success = false;
					result.ErrMessage = "Role Access Apps Not Found : Please contact administrator";
					return result;
				}

				//check token authorized
				if (string.IsNullOrEmpty(userName) || string.IsNullOrWhiteSpace(userName) ||
					string.IsNullOrEmpty(roleCode) || string.IsNullOrWhiteSpace(roleCode) ||
					string.IsNullOrEmpty(fullName) || string.IsNullOrWhiteSpace(fullName) ||
					Roles.Count < 1 ||
					string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
				{
					result.Success = false;
					result.ErrMessage = "Token Not Authorized";
					return result;
				}

				/*if(UserRole.Count() > 0)
				{
					//define claims identity
					List<Claim> claims = null;
					claims = new List<Claim> {
								new Claim("Username", userReq.Username),
								new Claim("FullName", fullName),
								new Claim("RoleCode", roleCode),
								new Claim("Email", email),
								new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
					 };

					UserRole.ForEach(c =>
					{
						claims.Add(new Claim(ClaimTypes.Role, c.RoleCode));
					});
				}*/

				//define claim identity
				ClaimsIdentity subject = new ClaimsIdentity(new Claim[]
				{
					new Claim("Username", userReq.Username),
					new Claim("FullName", fullName),
					new Claim("RoleCode", roleCode),
					new Claim("Email", email),
					//new Claim(ClaimTypes.Name, userReq.Username),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
				});

				//define role from get roles
				foreach (var item in UserRole)
				{
					subject.AddClaim(new Claim(ClaimTypes.Role, item.RoleCode));
				}

				//define token descriptor
				var tokenDescriptor = new SecurityTokenDescriptor
				{
					Subject = subject,
					Expires = DateTime.UtcNow.Add(_serviceConfiguration.JwtSettings.TokenLifeTime),
					SigningCredentials = credentials
					//SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
				};

				var token = tokenHandler.CreateToken(tokenDescriptor);
				result.Token = tokenHandler.WriteToken(token);

				//refresh token
				var refreshToken = new RefreshToken
				{
					Token = Guid.NewGuid().ToString(),
					JwtId = token.Id,
					UserId = userReq.Username,
					CreatedAt = DateTime.UtcNow,
					CreatedBy = "System",
					ExpireDate = DateTime.UtcNow.AddMinutes(10)
				};

				//set token result
				result.RefreshToken = refreshToken.Token;
				result.Success = true;

				return result;
			}
			catch (Exception ex)
			{
				result.Success = false;
				result.ErrMessage = "Something when wrong : " + ex.Message;
				return result;
			}
		}

        /*private static (bool Success, string Returns) GetAuthJwtFromPassport(string Url, string Body)
		{
			string response = null;
			try
			{
				System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(Url);
				request.Method = "POST";
				request.ContentType = "application/json";
				request.ContentLength = Body.Length;
				using (Stream webStream = request.GetRequestStream())
				using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
				{
					requestWriter.Write(Body);
				}

				System.Net.WebResponse webResponse = request.GetResponse();
				using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
				using (StreamReader responseReader = new StreamReader(webStream))
				{
					response = responseReader.ReadToEnd();
				}
				return (true, response);
			}
			catch (Exception e)
			{
				return (false, e.Message);
			}
		}*/

        public async Task< (bool Success, string Returns)> AuthenticationPasspost2(string url, UserModel.UserRequest userReq)
		{
            try
            {
                string json = JsonConvert.SerializeObject(userReq);
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var httpResponse = await _httpClientService.PostAsync(url, byteContent);

                if (httpResponse.Success)
                {
                    JObject finalResult = JObject.Parse(httpResponse.Returns);

                    if (!string.IsNullOrEmpty(finalResult["message"].ToString()))
                    {
                        return (false, "error : " + finalResult["message"].ToString());
                    }

					_httpClientService.SetToken(finalResult["data"]["token"].ToString());
                    return (true, finalResult["data"]["token"].ToString());
                }
                else
                {
                    return (false, "error");
                }
            }
            catch (Exception ex)
            {

                return (false, ex.Message);
            }
        }


        private static (bool Success, string Returns) AuthenticationPasspost(string url, UserModel.UserRequest userReq)
		{
			try
			{
				var handler = new HttpClientHandler();
				handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
				{
					// For development/testing, you might always return true to bypass validation
					// In production, you would implement proper validation logic based on 'errors'
					return true;
				};
				using (var client = new HttpClient(handler))
				{
					string json = JsonConvert.SerializeObject(userReq);
					byte[] buffer = Encoding.UTF8.GetBytes(json);
					var byteContent = new ByteArrayContent(buffer);
					byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
					var httpResponce = client.PostAsync(url, byteContent).Result;

					if (httpResponce.IsSuccessStatusCode)
					{
						var responseContent = httpResponce.Content;
						string result = responseContent.ReadAsStringAsync().Result;

						JObject finalResult = JObject.Parse(result);

						if (!string.IsNullOrEmpty(finalResult["message"].ToString()))
						{
							return (false, "error : " + finalResult["message"].ToString());
						}

						return (true, finalResult["data"]["token"].ToString());
					}
					else
					{
						return (false, "error");
					}
				}
			}
			catch (Exception ex)
			{

				return (false, ex.Message);
			}
			
		}

		private async Task<List<RoleModel>> GetRoles(UserModel.UserRequest user)
		{
			try
			{
				//get role by userid
				string query = @"SELECT RoleCode = A.RoleApplicationID,
                                RoleName = B.RoleAccessName,
                                UserCode = A.UserId
                                FROM RoleApplication A
                                INNER JOIN RoleAccess B ON A.RoleApplicationID = B.RoleAccessID
                                WHERE
                                A.UserID = @UserId";
				var param = new Dictionary<string, object>
				{
					{"@UserId", user.Username }
				};
				var data = await _repository.QueryListAsync<RoleModel>(query, param, false);

				return data;
			}
			catch (Exception)
			{

				return new List<RoleModel>();
			}
		}
	}
}
