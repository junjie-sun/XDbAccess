using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.Demo.Models;

namespace XDbAccess.Demo.Interfaces
{
    public interface IProductService
    {
        Task AddProductAsync(Product product);

        Task<List<Product>> GetAllProductsAsync();
    }
}
