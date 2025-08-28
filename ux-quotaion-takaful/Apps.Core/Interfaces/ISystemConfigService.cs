using DataAccess.EFCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Interfaces
{
    public interface ISystemConfigService : IRepository
    {
        Task<string> FindValueSysConfig(string sysCat, string sysSubCat, string sysCode);
    }
}
