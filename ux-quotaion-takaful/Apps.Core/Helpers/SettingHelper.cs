using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Helpers
{
	public class SettingHelper
	{
		private static IDictionary env;

		internal static string AppSettingValue(string name, string key, string envName = "")
		{
			string _envName;

			if (Environment.GetEnvironmentVariables() != null)
			{
				env = Environment.GetEnvironmentVariables();
			}


			if (env["ASPNETCORE_ENVIRONMENT"] == null)
			{
				_envName = envName;
			}
			else
			{
				_envName = "." + env["ASPNETCORE_ENVIRONMENT"].ToString().ToLower();

			}
			//if (env["ASPNETCORE_ENVIRONMENT"].ToString().Equals("Production", StringComparison.OrdinalIgnoreCase))
			//{
			//    _envName = string.Empty;
			//}

			string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { @"bin\" }, StringSplitOptions.None)[0];
			try
			{
				IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(projectPath)
				.AddJsonFile($"appsettings{_envName}.json")
				.Build();

				return configuration.GetSection(name)[key];

			}
			catch (Exception)
			{
				return null;
			}

		}

		internal static List<string> ListAppSettingValue(string name, string key)
		{
			if (Environment.GetEnvironmentVariables() != null)
			{
				env = Environment.GetEnvironmentVariables();
			}

			string envName = "";
			if (env["ASPNETCORE_ENVIRONMENT"] != null)
			{
				envName = "." + env["ASPNETCORE_ENVIRONMENT"].ToString().ToLower();
			}
			if (env["ASPNETCORE_ENVIRONMENT"].ToString().Equals("Production", StringComparison.OrdinalIgnoreCase))
			{
				envName = string.Empty;
			}

			string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { @"bin\" }, StringSplitOptions.None)[0];
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(projectPath)
				.AddJsonFile($"appsettings{envName}.json")
				.Build();
			var list = configuration.GetSection($"{name}:{key}").Get<List<string>>();
			if (list == null)
			{
				return new List<string>();
			}
			else
			{

				return list;

			}
		}

		internal static int AppSettingValue(string name, string key, int defValue)
		{
			if (Environment.GetEnvironmentVariables() != null)
			{
				env = Environment.GetEnvironmentVariables();
			}

			string envName = "";
			if (env["ASPNETCORE_ENVIRONMENT"] != null)
			{
				envName = "." + env["ASPNETCORE_ENVIRONMENT"].ToString().ToLower();
			}
			if (env["ASPNETCORE_ENVIRONMENT"].ToString().Equals("Production", StringComparison.OrdinalIgnoreCase))
			{
				envName = string.Empty;
			}
			string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { @"bin\" }, StringSplitOptions.None)[0];
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(projectPath)
				.AddJsonFile($"appsettings{envName}.json")
				.Build();

			if (configuration.GetSection(name)[key] == null)
			{
				return defValue;
			}
			else
			{
				return int.Parse(configuration.GetSection(name)[key]);
			}
		}

		internal static bool AppSettingValue(string name, string key, bool defValue)
		{
			bool defval = false;
			if (Environment.GetEnvironmentVariables() != null)
			{
				env = Environment.GetEnvironmentVariables();
			}

			string envName = "";
			if (env["ASPNETCORE_ENVIRONMENT"] != null)
			{
				envName = "." + env["ASPNETCORE_ENVIRONMENT"].ToString().ToLower();
			}
			if (env["ASPNETCORE_ENVIRONMENT"].ToString().Equals("Production", StringComparison.OrdinalIgnoreCase))
			{
				envName = string.Empty;
			}
			string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { @"bin\" }, StringSplitOptions.None)[0];
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(projectPath)
				.AddJsonFile($"appsettings{envName}.json")
				.Build();

			if (configuration.GetSection(name)[key] == null)
			{
				return defValue;
			}
			else
			{
				return bool.TryParse(configuration.GetSection(name)[key], out defval);
			}
		}
	}

	public class Others
	{
		public static string CompressString(string text)
		{
			byte[] buffer = Encoding.UTF8.GetBytes(text);
			var memoryStream = new MemoryStream();
			using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
			{
				gZipStream.Write(buffer, 0, buffer.Length);
			}

			memoryStream.Position = 0;

			var compressedData = new byte[memoryStream.Length];
			memoryStream.Read(compressedData, 0, compressedData.Length);

			var gZipBuffer = new byte[compressedData.Length + 4];
			Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
			Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
			return Convert.ToBase64String(gZipBuffer);
		}
	}
}
