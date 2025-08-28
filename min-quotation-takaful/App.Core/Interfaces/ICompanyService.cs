using App.Core.Models.Company;
using SharedKernel.Entities;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface ICompanyService : IRepository
    {
        Task<(bool added, string message)> InsertCompanyAgent(CompanyRequest model);
        Task<(bool added, string message)> ApproveCompanyAgent(CompanyApproveRequest model);
        Task<(bool added, string message)> RemoveCompanyAgent(CompanyApproveRequest model);
        Task<(bool valid, string message)> ValidateCompanyAgent(CompanyRequest model);
        Task<List<CompanyViewModel>> GetCompanyAgents(string userId, string companyCode = "");
        Task<List<CompanyViewModel>> GetSearchCompanyAgents(string userId, CompanySearchViewModel model);
        Task<List<CompanyViewModel>> GetSearchCompanyApproval(string userId, CompanySearchViewModel model);
    }
}
