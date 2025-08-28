using Apps.Core.DTOS;
using DataAccess.EFCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Interfaces
{
    public interface IRoleService : IRepository
    {
        Task<List<RoleApplicationDTO>> GetRoleApplicationByUserId(string userId, bool isAdmin = false);
        Task<List<RoleDTO>> GetAllRole();
        Task<bool> RemoveRoleByUserId(string userId);
        Task<bool> AddRoleApplication(string userId, string[] role);
    }
}
