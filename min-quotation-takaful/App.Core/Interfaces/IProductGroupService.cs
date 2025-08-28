using App.Core.Models.ProductGroup;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface IProductGroupService : IRepository
    {
        Task<List<ProductGroup>> GetAll();
    }
}
