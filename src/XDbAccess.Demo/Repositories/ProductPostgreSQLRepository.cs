using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using XDbAccess.Dapper;
using XDbAccess.Demo.Interfaces;
using XDbAccess.Demo.Models;
using System.Linq;

namespace XDbAccess.Demo.Repositories
{
    public class ProductPostgreSQLRepository : BaseRepository<DapperTest2DbContext>, IProductRepository
    {
        public ProductPostgreSQLRepository(IDbHelper<DapperTest2DbContext> dbHelper) : base(dbHelper) { }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            IEnumerable<Product> data;
            string sql = "select * from \"Product\"";
            data = await DbHelper.QueryAsync<Product>(sql);
            return data.ToList();
        }

        public async Task InsertProductAsync(Product product)
        {
            product.Id = Convert.ToInt32(await DbHelper.InsertAsync<Product>(product));
        }
    }
}
