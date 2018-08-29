using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.Demo.Interfaces;
using XDbAccess.Demo.Models;
using XDbAccess.Demo.Repositories;

namespace XDbAccess.Demo.Services
{
    public class ProductService : IProductService
    {
        private IProductRepository _ProductRepo;

        public ProductService(IProductRepository productRepo)
        {
            _ProductRepo = productRepo;
        }

        public async Task AddProductAsync(Product product)
        {
            await _ProductRepo.InsertProductAsync(product);
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _ProductRepo.GetAllProductsAsync();
        }
    }
}
