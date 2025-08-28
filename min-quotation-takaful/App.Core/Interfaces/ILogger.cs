using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    // Interface Segregation Principle (ISP): Separate logger interface for specific logging needs.
    public interface ILogger
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);
        Task LogCreate(string processid, string processName, string userId, string description, string readFlag, string processState);
    }
}
