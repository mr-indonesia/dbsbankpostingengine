using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Models.Authentication
{
	public class AuthenticationResult : TokenModel
	{
		public bool Success { get; set; }
		public string ErrMessage {  get; set; }
		public IEnumerable<string> Error { get; set; }
	}

	public class TokenModel
	{
		[JsonProperty("token")]
		public string Token { get; set; }

		[JsonProperty("refreshToken")]
		public string RefreshToken { get; set; }
	}
}
