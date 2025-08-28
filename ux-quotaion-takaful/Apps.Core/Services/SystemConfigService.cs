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

namespace Apps.Core.Services
{
    public class SystemConfigService : EFRepository, ISystemConfigService
    {
        private readonly IRepository _repository;
        private readonly ServiceConfiguration _serviceConfiguration;

        public SystemConfigService(ApplicationContext context, IRepository repository, IOptions<ServiceConfiguration> serviceConfiguration) : base(context)
        {
            this._repository = repository;
            _serviceConfiguration = serviceConfiguration.Value;
        }

        public async Task<string> FindValueSysConfig(string sysCat, string sysSubCat, string sysCode)
        {
            try
            {
                string query = $@"SELECT SystemValue FROM MasterSystemConfig
                                WHERE SystemCategory = @SysCat 
                                AND SystemSubCategory = @SysSubCat 
                                AND SystemCode = @SysCode";

                var param = new Dictionary<string, object> {
                    { "@SysCat", sysCat},
                    { "@SysSubCat", sysSubCat},
                    { "@SysCode", sysCode},
                };

                var data = (await _repository.QueryListAsync<string>(query, param, false)).FirstOrDefault();

                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
