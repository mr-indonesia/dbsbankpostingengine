using Apps.Core.DTOS;
using Apps.Core.Models;
using DataAccess.EFCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Interfaces
{
    public interface ICompanyBranchService : IRepository
    {
        Task<CompanyInfo> FindCompanyById(string companyId);
        Task<List<BranchDTO>> FindAllBranch();
        Task<List<DdlBranch>> FindDdlBranch();
        Task<List<CompanyUserDTO>> FindBranchByUserId(string userId);
    }
}
