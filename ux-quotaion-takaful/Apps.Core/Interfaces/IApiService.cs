using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Interfaces
{
    public interface IApiService
    {
        Task<(bool Success, string Returns)> GetAsync(string url);
        Task<(bool Success, string Returns)> PostAsync(string url, HttpContent content);
        void SetToken(string token);
    }
}
