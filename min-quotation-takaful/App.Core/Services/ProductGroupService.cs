using App.Core.Interfaces;
using App.Core.Models.Email;
using App.Core.Models.ProductGroup;
using DataAccess.EFCore;
using DataAccess.EFCore.Repositories;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Services
{
    public class ProductGroupService : EFRepository, IProductGroupService
    {
        private readonly IRepository _repository;
        public ProductGroupService(ApplicationContext context, IRepository repository) : base(context)
        {
            this._repository = repository;
        }

        public async Task<List<ProductGroup>> GetAll()
        {
            try
            {
                string query = @"SELECT RoleCode, Description FROM MstRoles	";

                var data = await _repository.QueryAsync<ProductGroup>(query, null, false);

                return data;
            }
            catch (Exception)
            {

                return new List<ProductGroup>();
            }
        }
    }
}
