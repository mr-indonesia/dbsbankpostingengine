using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Models;
using App.Core.Models.Authentications;
using App.Core.Models.Users;
using nusantaratech.System.Data;
using SharedKernel.Entities;
using SharedKernel.Interfaces;

namespace App.Core.Interfaces
{
    public interface IIdentityService : IRepository
    {
        Task<ResponseModel<TokenModel>> Login(string username, string password);
        Task<ResponseData<TokenModel>> LoginUser(string username, string password);
        Task<AuthenticationResult> AuthecticateAsync(UserModel user);
    }
}
