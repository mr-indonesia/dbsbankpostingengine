using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace DataAccess.EFCore.Interfaces
{
    public interface IRepository
    {
        IEnumerable<T> GetAll<T>() where T : class;
        Task<IEnumerable<T>> GetAllAsync<T>() where T : class;
        T GetById<T>(int id) where T : class;
        Task<T> GetByIdAsync<T>(object id) where T : class;
        List<T> QueryList<T>(string query, Dictionary<string, object> param = null, bool isStoredProcedure = false);
        void ExecuteQuery(string query, Dictionary<string, object> param = null, bool isStoredProcedure = false);
        IEnumerable<T> ExecuteProcedure<T>(string spName, object param);
        List<object> ExecuteMultipleProcedure(string spName, object param, params Func<GridReader, object>[] readerFuncts);
        Task<List<T>> QueryListAsync<T>(string query, Dictionary<string, object> param, bool isstoredProcedure = false);
        Task ExecuteQueryAsync(string query, object param, bool isstoredProcedure = false);
        Task ExecuteQueryAsync(string query, Dictionary<string, object> param, bool isstoredProcedure = false);
        Task<List<T>> ListAsyncWithWhere<T>(Expression<Func<T, bool>> expression) where T : class;
    }
}
