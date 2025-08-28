using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Models.Authentication
{
	public class ServiceConfiguration
	{
		public JwtSettings JwtSettings { get; set; }
		public Server Server { get; set; }
	}

	public class JwtSettings
	{
		public string Secret { get; set; }
		public TimeSpan TokenLifeTime { get; set; }
		public string Issuer { get; set; }
	}

	public class Server
	{
		public string APIUrl { get; set; }
		public string PassportUrl { get; set; }
	}
}
