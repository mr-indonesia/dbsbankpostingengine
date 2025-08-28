using Apps.Core.Data;
using Apps.Core.Helpers;
using Apps.Core.Interfaces;
using Apps.Core.Models.Authentication;
using Apps.Core.Services;
using Apps.Core.UnitOfWorks;
using AutoMapper;
using AutoMapper.Data;
using DataAccess.EFCore;
using DataAccess.EFCore.Interfaces;
using DataAccess.EFCore.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApps
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			//to handle json
			#region Configure JSON Serializer Options and NewTonSoftJson
			services.AddControllersWithViews().
				AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
					options.JsonSerializerOptions.PropertyNamingPolicy = null;
					options.JsonSerializerOptions.AllowTrailingCommas = true;
					options.JsonSerializerOptions.WriteIndented = false; // Reduce size
					options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
					options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
					//options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				})
			.AddNewtonsoftJson(o =>
			 {
				 o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
				 o.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
				 o.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
				 o.SerializerSettings.Formatting = Formatting.None;
				 o.SerializerSettings.ContractResolver = new DefaultContractResolver
				 {
					 NamingStrategy = null // Menggunakan nama properti asli
				 };
			 });

			services.Configure<IISServerOptions>(options =>
			{
				options.MaxRequestBodySize = 30_000_000; // ~30MB
			});
            #endregion

            #region Configure HttpClient
            /*services.AddHttpClient("AuthenticatedClient", client =>
            {
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            })
			.AddHttpMessageHandler<AuthHeaderHandler>();

            services.AddTransient<AuthHeaderHandler>();
            services.AddScoped<IApiService, ApiService>();*/

            // Add HttpClient with authentication handler
            /*services.AddHttpClient<IApiService, ApiService>()
                    .AddHttpMessageHandler<AuthenticationHandler>();*/

            //services.AddHttpClient<IApiService, ApiService>(client =>
            //{
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(
            //        new MediaTypeWithQualityHeaderValue("application/json"));
            //    client.Timeout = TimeSpan.FromSeconds(30);
            //});
            #endregion

            #region Configure automapper
            //services.AddAutoMapper(typeof(Startup));
            //var config = new MapperConfiguration(cfg =>
            //{
            //	cfg.AddMaps(typeof(Startup).Assembly);
            //});
            //services.AddSingleton<IMapper>(config.CreateMapper());

            //for custom profiling
            var mappingConfig = new MapperConfiguration(mc =>
			{
				// enable automatic IDataReader Mapping, register mapping at MappingProfileExt
				mc.AddDataReaderMapping();
				//mc.AddDataReaderProfile(new MappingProfile());
				mc.AddProfile(new MappingProfile());

			});
			services.AddSingleton(mappingConfig.CreateMapper());
			#endregion

			#region Configure Response Compression
			services.AddResponseCompression(options =>
			{
				options.EnableForHttps = true;
				options.Providers.Add<BrotliCompressionProvider>();
				options.Providers.Add<GzipCompressionProvider>();
			});

			// Configure compression levels if needed
			services.Configure<BrotliCompressionProviderOptions>(options =>
			{
				options.Level = CompressionLevel.Fastest;
			});

			services.Configure<GzipCompressionProviderOptions>(options =>
			{
				options.Level = CompressionLevel.SmallestSize;
			});
			#endregion

			//setting max file size
			services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
                options.KeyLengthLimit = int.MaxValue;
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = 50000000; // Limit to 5 MB

            });

			#region Security Setting Avoid AntiForgeyToken and addhsts
			//enforce HTTPS
			services.AddHttpsRedirection(options =>
			{
				options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
				options.HttpsPort = 443;
			});

			//add middleware HTTP Strict Transport Security
			services.AddHsts(options =>
			{
				//options.Preload = true;
				options.IncludeSubDomains = true;
				options.MaxAge = TimeSpan.FromDays(365);
				//options.ExcludedHosts.Clear();
			});

			services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);

			//services.AddAntiforgery(options =>
			//{
			//	options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
			//	options.SuppressXFrameOptionsHeader = true;
			//	options.Cookie.SameSite = SameSiteMode.Strict;
			//	options.Cookie.HttpOnly = true;
			//	options.HeaderName = "X-CSRF-TOKEN";
			//});
			#endregion

			#region Setting Cookie Policy
			services.Configure<CookiePolicyOptions>(options =>
			{
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
				// Ensure cookies are only sent over HTTPS
				options.Secure = CookieSecurePolicy.Always;

				// Configure SameSite settings
				options.MinimumSameSitePolicy = SameSiteMode.Lax; // or Strict

				// Handle legacy browsers that don't support SameSite
				options.OnAppendCookie = cookieContext =>
					CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
				options.OnDeleteCookie = cookieContext =>
					CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
			});
			#endregion

			#region Register ApplicationContext
			services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")
                , b => b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
			#endregion

			#region Register Configuration From appsettings
			//configure strongly typed settings object
			var appSettingsSection = Configuration.GetSection("ServiceConfiguration");
			services.Configure<ServiceConfiguration>(appSettingsSection);
			#endregion Register Configuration

			#region Configure JWT AUTH
			var serviceConfiguration = appSettingsSection.Get<ServiceConfiguration>();
			var jwtSecretKey = Encoding.UTF8.GetBytes(serviceConfiguration.JwtSettings.Secret);
			var jwtIssuer = serviceConfiguration.JwtSettings.Issuer;
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(jwtSecretKey),
				ValidateIssuer = false, //set true
				ValidateAudience = false, //set true
										  //ValidIssuer = jwtIssuer,
										  //ValidAudience = jwtIssuer,
				RequireExpirationTime = false,
				ValidateLifetime = true //set false
			};
			services.AddSingleton(tokenValidationParameters);
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = tokenValidationParameters;
			});

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.Cookie.SameSite = SameSiteMode.Strict;
					options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
					options.Cookie.IsEssential = true;
				});

			services.AddSession(options =>
			{
				options.Cookie.SameSite = SameSiteMode.Strict;
				options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
				options.Cookie.IsEssential = true;
			});

			//services.AddSession(options => {
			//	options.IdleTimeout = TimeSpan.FromMinutes(15);
			//	//options.Cookie.SecurePolicy = true;
			//	options.Cookie.SameSite = SameSiteMode.Lax;
			//	options.Cookie.HttpOnly = true;
			//	options.Cookie.IsEssential = true;
			//});

			services.ConfigureApplicationCookie(options =>
			{
				options.Cookie.SameSite = SameSiteMode.Strict;
			});

			services.AddCors(options =>
			{
				options.AddPolicy("AllowSpecificOrigin", builder =>
								builder.AllowAnyOrigin()
								.AllowAnyMethod()
								.AllowAnyHeader());
			});
			#endregion Configure JWT AUTH

			#region Register All Interface and Repository
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRepository, EFRepository>();
			services.AddSingleton<IApiService, ApiService>();

			services.AddTransient<IIdentityService, IdentityService>();
			services.AddScoped<IUserMappingService, UserMappingService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICompanyBranchService, CompanyBranchService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserInfoService, UserInfoService>();
            services.AddScoped<IWorkflowService, WorkflowService>();
            services.AddScoped<IReferenceTableService, ReferenceTableService>();
            services.AddScoped<IRoleAccessService, RoleAccessService>();
            services.AddScoped<IMigrationService, MigrationService>();
            services.AddScoped<ICompanyAgentService, CompanyAgentService>();
            services.AddScoped<ISystemConfigService, SystemConfigService>();
			#endregion

			#region BYPASS SSL
			services.AddHttpClient("UnsafeClient")
			.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
			});
			#endregion			

			

			#region Configure Authentication Cookies
			//services.ConfigureApplicationCookie(options =>
			//{
			//	options.Cookie.SameSite = SameSiteMode.Lax;
			//	options.Cookie.HttpOnly = true;
			//	options.Cookie.IsEssential = true;
			//});

			// If using external authentication
			//services.ConfigureExternalCookie(options =>
			//{
			//	//options.Cookie.Secure = true;
			//	options.Cookie.SameSite = SameSiteMode.Lax;
			//	options.Cookie.HttpOnly = true;
			//});
			#endregion

			#region setting httpcontextaccessor and session
			services.AddHttpContextAccessor();            
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region AppHttpContext
            AppsHttpContext.serviceProvider = app.ApplicationServices;
            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

			#region Configure Cors
			//to activate Cross-Origin Resource Sharing (CORS)
			app.UseCors(x => x
			 .AllowAnyOrigin()
			 .AllowAnyMethod()
			 .AllowAnyHeader());
			#endregion

			#region Setting Cookies
			//https://www.codeproject.com/Articles/1259066/10-Points-to-Secure-Your-ASP-NET-Core-MVC-Applicat
			app.UseCookiePolicy(new CookiePolicyOptions
			{
				HttpOnly = HttpOnlyPolicy.Always,
				Secure = CookieSecurePolicy.Always,
				MinimumSameSitePolicy = SameSiteMode.None
			});			
			#endregion

			app.UseStaticFiles();

            app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			#region add CSP security
			app.Use(async (context, next) =>
			{
				if (env.IsDevelopment() || env.IsProduction())
				{
					context.Response.OnStarting(() =>
					{
						//https://stackoverflow.com/questions/39174888/asp-net-core-remove-x-powered-by-cannot-be-done-in-middleware
						//https://stackoverflow.com/questions/79338418/removing-server-header-not-working-in-asp-net-core-8-does-not-work-with-middle
						context.Response.Headers.Remove("Server");
						context.Response.Headers.Remove("X-AspNetWebPages-Version");
						context.Response.Headers.Remove("X-AspNet-Version");
						context.Response.Headers.Remove("X-Powered-By");
						context.Response.Headers.Remove("X-AspNetMvc-Version");

						return Task.CompletedTask;
					});


					//Anti-clickjacking Header
					//https://dzone.com/articles/secure-net-core-applications-from-click-jacking-ne
					context.Response.Headers.Append("X-Frame-Options", "DENY");
					context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");

					//https://stackoverflow.com/questions/71209211/how-to-add-to-a-asp-net-core-net6-project-owasp-recommendation
					context.Response.Headers.Append("X-Content-Type-Options", "nosniff");

					//https://www.stackhawk.com/blog/net-content-security-policy-guide-what-it-is-and-how-to-enable-it/
					//context.Response.Headers.Add("Content-Security-Policy", "default-src 'self';");
					//context.Response.Headers.Append("Content-Security-Policy", "default-src 'self';");
					context.Response.Headers.Append("Content-Security-Policy", "default-src 'self' https: 'unsafe-inline' 'unsafe-eval'; img-src 'self' data: ;font-src 'self' data:;");

					//context.Response.Headers.Remove("X-AspNetMvc-Version");                    
					context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload");

					//From Engine AI
					context.Response.Headers.Append("Referrer-Policy", "no-referrer");
					context.Response.Headers.Append("X-Permitted-Cross-Domain-Policies", "none");

					context.Response.Headers.Append("access-control-allow-credentials", "true");
					context.Response.Headers.Append("access-control-allow-headers", "Authorization, Content-Type, Origin, x-requested-with, x-signalr-user-agent");
					context.Response.Headers.Append("access-control-allow-methods", "POST, PUT, GET, PATCH, DELETE, OPTIONS");
					//context.Response.Headers.Append("access-control-allow-origin", AllowedOrigin);
					context.Response.Headers.Append("access-control-max-age", "3600");

					context.Response.Headers.Append("Feature-Policy", "accelerometer 'none'; camera 'none'; geolocation 'none'; gyroscope 'none'; magnetometer 'none'; microphone 'none'; payment 'none'; usb 'none'");

					await next();
				}
			});
			#endregion

			#region add use sesion
			app.UseSession();
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
            });
        }

		private void CheckSameSite(HttpContext httpContext, CookieOptions options)
		{
			if (options.SameSite == SameSiteMode.None)
			{
				var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
				if (DisallowsSameSiteNone(userAgent))
				{
					options.SameSite = SameSiteMode.Unspecified;
				}
			}
		}

		private bool DisallowsSameSiteNone(string userAgent)
		{
			// Check for user agents that don't support SameSite=None
			// This is a simplified version - you may need more comprehensive checks
			if (userAgent.Contains("CPU iPhone OS 12") ||
				userAgent.Contains("iPad; CPU OS 12") ||
				userAgent.Contains("Macintosh; Intel Mac OS X 10_14") ||
				userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
			{
				return true;
			}
			return false;
		}
	}
}
