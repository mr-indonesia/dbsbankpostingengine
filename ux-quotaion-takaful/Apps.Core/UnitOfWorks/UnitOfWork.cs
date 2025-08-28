using Apps.Core.Interfaces;
using Apps.Core.Models.Authentication;
using Apps.Core.Services;
using AutoMapper;
using DataAccess.EFCore;
using DataAccess.EFCore.Interfaces;
using DataAccess.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        public UnitOfWork(ApplicationContext context, IRepository repository, IOptions<ServiceConfiguration> _serviceConfiguration, TokenValidationParameters _tokenValidationParameters, IApiService apiService)
        {
            _context = context;
            UserMappingService = new UserMappingService(_context, repository);
            UserService = new UserService(_context, repository);
            CompanyBranchService = new CompanyBranchService(_context, repository);
            RoleService = new RoleService(_context, repository);
            UserInfoService = new UserInfoService(_context, repository);
            WorkflowService = new WorkflowService(_context, repository);
            ReferenceTableService = new ReferenceTableService(_context, repository, apiService);
            RoleAccessService = new RoleAccessService(_context, repository);
			IdentityService = new IdentityService(context, repository, _serviceConfiguration, _tokenValidationParameters, apiService);
            MigrationService = new MigrationService(_context, repository);
            CompanyAgentService = new CompanyAgentService(_context, repository, apiService);
            SystemConfigService = new SystemConfigService(_context, repository, _serviceConfiguration);
		}

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        private bool _disposed = false;

        public IUserMappingService UserMappingService { get; private set;}

        public IUserService UserService { get; private set; }
        public ICompanyBranchService CompanyBranchService { get; private set; }
        public IRoleService RoleService { get; private set; }

        public IUserInfoService UserInfoService { get; private set; }
        public IWorkflowService WorkflowService { get; private set; }

        public IReferenceTableService ReferenceTableService { get; private set; }

		public IRoleAccessService RoleAccessService { get; private set; }

		public IIdentityService IdentityService { get; private set; }

		public IMigrationService MigrationService { get; private set; }

        public ICompanyAgentService CompanyAgentService { get; private set; }

        public ISystemConfigService SystemConfigService { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
