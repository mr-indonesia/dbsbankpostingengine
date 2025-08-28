using Dapper;
using DataAccess.EFCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataAccess.EFCore.Repositories
{
    public class EFRepository : IRepository
    {
        protected readonly ApplicationContext context;
        public EFRepository(ApplicationContext _context)
        {
            context = _context ?? throw new ArgumentNullException(nameof(_context));
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            return context.Set<T>().ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
        {
            return await context.Set<T>().ToListAsync();
        }

        public T GetById<T>(int id) where T : class
        {
            return context.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync<T>(object id) where T : class
        {
            return await context.Set<T>().FindAsync(id);
        }

        public List<T> QueryList<T>(string query, Dictionary<string, object> param = null, bool isStoredProcedure = false)
        {
            if(isStoredProcedure)
            {
                return (context.Database.GetDbConnection().Query<T>(query, param, commandTimeout: 120, commandType: CommandType.StoredProcedure)).ToList();
            }
            return (context.Database.GetDbConnection().Query<T>(query, param, commandTimeout: 120)).ToList();
        }

        public void ExecuteQuery(string query, Dictionary<string, object> param = null, bool isStoredProcedure = false)
        {
            if (isStoredProcedure)
            {
                context.Database.GetDbConnection().Execute(query, param, commandTimeout: 120, commandType: CommandType.StoredProcedure);
            }
            context.Database.GetDbConnection().Execute(query, param, commandTimeout: 120);
        }

        public IEnumerable<T> ExecuteProcedure<T>(string spName, object param)
        {
            return context.Database.GetDbConnection().Query<T>(spName, param, commandTimeout: 120, commandType: CommandType.StoredProcedure);
        }

        public List<object> ExecuteMultipleProcedure(string spName, object param, params Func<SqlMapper.GridReader, object>[] readerFuncts)
        {
            var result = new List<object>();
            var gridReader = context.Database.GetDbConnection().QueryMultiple(spName, param, commandTimeout: 120, commandType: CommandType.StoredProcedure);

            foreach (var reader in readerFuncts)
            {
                var obj = reader(gridReader);
                result.Add(obj);
            }

            return result;
        }

        public async Task<List<T>> QueryListAsync<T>(string query, Dictionary<string, object> param, bool isstoredProcedure = false)
        {
            try
            {
                if (isstoredProcedure)
                {
                    return (await context.Database.GetDbConnection().QueryAsync<T>(query, param, commandTimeout: 120, commandType: CommandType.StoredProcedure)).ToList();
                }
                return (await context.Database.GetDbConnection().QueryAsync<T>(query, param, commandTimeout: 120)).ToList();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task ExecuteQueryAsync(string query, object param, bool isstoredProcedure = false)
        {
            try
            {
                if (isstoredProcedure)
                {
                    await context.Database.GetDbConnection().ExecuteAsync(query, param, commandTimeout: 120, commandType: CommandType.StoredProcedure);
                }
                await context.Database.GetDbConnection().ExecuteAsync(query, param, commandTimeout: 120);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task ExecuteQueryAsync(string query, Dictionary<string, object> param, bool isstoredProcedure = false)
        {
            try
            {
                if (isstoredProcedure)
                {
                    await context.Database.GetDbConnection().ExecuteAsync(query, param, commandTimeout: 120, commandType: CommandType.StoredProcedure);
                }
                await context.Database.GetDbConnection().ExecuteAsync(query, param, commandTimeout: 120);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<T>> ListAsyncWithWhere<T>(Expression<Func<T, bool>> expression) where T : class
        {
            try
            {
                var Query = context.Set<T>() as IQueryable<T>;
                Query = Query.Where(expression);

                return await Query.ToListAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
