using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserMappingService UserMappingService { get; }
        IUserService UserService { get; }
        ICompanyBranchService CompanyBranchService { get; }
        IRoleService RoleService { get; }
        IUserInfoService UserInfoService { get; }
        IWorkflowService WorkflowService { get; }
        IReferenceTableService ReferenceTableService { get; }
		IRoleAccessService RoleAccessService { get; }
		IIdentityService IdentityService { get; }
		IMigrationService MigrationService { get; }
        ICompanyAgentService CompanyAgentService { get; }
        ISystemConfigService SystemConfigService { get; }
        Task<int> Complete();
    }
}
