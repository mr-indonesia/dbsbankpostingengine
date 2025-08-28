using Apps.Core.Models.Company;
using DataAccess.EFCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Interfaces
{
    public interface ICompanyAgentService : IRepository
    {
        Task<(bool Success, List<CompanyViewModel> Returns)> GetAllCompanyAgent(string url);
        Task<(bool Success, string Returns)> AddCompanyAgent(string url, CompanyRequest model);
        Task<(bool Success, string Returns)> ApproveCompanyAgent(string url, CompanyApproveRequest model);
        Task<(bool Success, string Returns)> ValidateCompanyAgent(string url, CompanyRequest model);
    }
}
