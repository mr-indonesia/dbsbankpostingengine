using Apps.Core.DTOS;
using DataAccess.EFCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Interfaces
{
    public interface IUserInfoService : IRepository
    {
        Task<List<UserDTO>> GetAllUser(string search);
        Task<UserDTO> FindUserinfoById(string id);
        Task<bool> UpdateUserInfo(UserDTO model);
    }
}
