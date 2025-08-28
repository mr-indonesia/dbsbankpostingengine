using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace WebApps
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
					.ConfigureKestrel((context, options) =>
					{
						// Set properties and call methods on options
						options.ConfigureHttpsDefaults(s =>
						{
							s.SslProtocols = SslProtocols.Tls12;
						});
						options.Limits.MaxResponseBufferSize = 1024 * 1024 * 100; // 100MB
					})
					.UseUrls("http://*:5000", "https://*:5001")
					.UseContentRoot(Directory.GetCurrentDirectory())
					.UseIISIntegration()
					.UseStartup<Startup>()
					.ConfigureLogging(logging =>
					{
						logging.ClearProviders();
						logging.AddConsole();
						logging.AddDebug();
						// logging.AddAzureWebAppDiagnostics(); add this if deploying to Azure
					});
				});
    }
}
