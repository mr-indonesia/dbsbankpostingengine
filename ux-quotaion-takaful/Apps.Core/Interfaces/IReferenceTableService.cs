using Apps.Core.Models;
using DataAccess.EFCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Interfaces
{
    public interface IReferenceTableService : IRepository
    {
        Task<List<DdlGroupProduct>> FindDllProductGroup();
        Task<List<DdlRoleAccess>> FindDdlRoleAcces();
        Task<List<DdlAmount>> FindDdlAmount();
        Task<List<DdlUser>> FindDdlUser();
        Task<List<DdlCompanyType>> FindDdlCompanyType(string url);
        Task<List<DdlCompanyLOB>> FindDdlCompanyLOB(string url);
        Task<List<DdlCompanyCategory>> FindDdlCompanyCategory(string url);
        Task<List<DdlCompanyStatus>> FindDdlCompanyStatus(string url);
        Task<List<DdlPropinsi>> FindDdlPropinsi(string url);

	}
}
