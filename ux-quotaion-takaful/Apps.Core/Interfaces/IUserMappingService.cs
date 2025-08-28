using Apps.Core.DTOS;
using DataAccess.EFCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Interfaces
{
    public interface IUserMappingService : IRepository
    {
        Task<List<UserDTO>> GetAllUserMapping();
        Task<Result<UserMappingResponse>> CreateUserMappingAsync(UserMappingRequest request);
        //Task<UserMappingResponse> CreateUserMappingAsync(UserMappingRequest request);
        Task<UserMappingResponse> UpdateUserMappingAsync(string userId, UserMappingRequest request);
        Task<bool> DeleteUserMappingAsync(string userId);
        Task<UserMappingResponse> GetUserMappingAsync(string userId);
    }
}
