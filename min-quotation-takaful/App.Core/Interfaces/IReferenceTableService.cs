using App.Core.Models;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface IReferenceTableService : IRepository
    {
        Task<List<DdlGroupProduct>> FindDllProductGroup();
        Task<List<DdlRoleAccess>> FindDdlRoleAcces();
        Task<List<DdlAmount>> FindDdlAmount();
        Task<List<DdlUser>> FindDdlUser();
        Task<List<DdlCompanyType>> FindDdlCompanyType();
        Task<List<DdlCompanyLOB>> FindDdlCompanyLOB();
        Task<List<DdlCompanyCategory>> FindDdlCompanyCategory();
        Task<List<DdlCompanyStatus>> FindDdlCompanyStatus();
        Task<List<DdlCompanyStatus>> FindDdlCompanyStatus(string code);
        Task<List<DdlPropinsi>> FindDdlPropinsi();
    }
}
