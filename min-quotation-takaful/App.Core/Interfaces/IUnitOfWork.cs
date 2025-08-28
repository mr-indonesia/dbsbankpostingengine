using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> Complete();
        public IMailSender MailSender { get; }
        public IIdentityService IdentityService { get; }
        public IProductGroupService ProductGroupService { get; }
        public IMigrationService MigrationService { get; }
        public IReferenceTableService ReferenceTableService { get; }
        public ICompanyService CompanyService { get; }
    }
}
