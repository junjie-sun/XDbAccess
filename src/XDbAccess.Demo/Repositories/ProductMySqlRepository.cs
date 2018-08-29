using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.Demo.Interfaces;
using XDbAccess.Demo.Models;
using XDbAccess.Dapper;

namespace XDbAccess.Demo.Repositories
{
    public class ProductMySqlRepository : BaseRepository<DapperTest2DbContext>, IProductRepository
    {
        public ProductMySqlRepository(IDbHelper<DapperTest2DbContext> dbHelper) : base(dbHelper) { }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            IEnumerable<Product> data;
            string sql = "select * from `Product`";
            data = await DbHelper.QueryAsync<Product>(sql);
            return data.ToList();
        }

        public async Task InsertProductAsync(Product product)
        {
            product.Id = Convert.ToInt32(await DbHelper.InsertAsync<Product>(product));
        }
    }
}
