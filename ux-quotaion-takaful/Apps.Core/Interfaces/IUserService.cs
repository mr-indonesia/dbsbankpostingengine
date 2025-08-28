using DataAccess.EFCore.Entities;
using DataAccess.EFCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Interfaces
{
    public interface IUserService : IRepository
    {
        Task<User> GetUserById(string userid);

	}
}
