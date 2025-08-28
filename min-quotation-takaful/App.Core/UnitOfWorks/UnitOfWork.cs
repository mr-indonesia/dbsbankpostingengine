using App.Core.Interfaces;
using App.Core.Models.Authentications;
using App.Core.Services;
using AutoMapper;
using DataAccess.EFCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        public UnitOfWork(ApplicationContext context, IRepository _repository, IOptions<ServiceConfiguration> _serviceConfiguration, TokenValidationParameters _tokenValidationParameters)
        {
            _context = context;
            MailSender = new MailSenderService(context, _repository);
            IdentityService = new IdentityService(context, _repository, _serviceConfiguration, _tokenValidationParameters);
            ProductGroupService = new ProductGroupService(context, _repository);
            MigrationService = new MigrationService(context, _repository);
            ReferenceTableService = new ReferenceTableService(context, _repository);
            CompanyService = new CompanyService(context, _repository);
        }
        public IMailSender MailSender { get; private set; }

        public IIdentityService IdentityService { get; private set; }
        public IProductGroupService ProductGroupService { get; private set; }

		public IMigrationService MigrationService { get; private set; }

        public IReferenceTableService ReferenceTableService { get; private set; }

        public ICompanyService CompanyService { get; private set; }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        private bool _disposed = false;
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
