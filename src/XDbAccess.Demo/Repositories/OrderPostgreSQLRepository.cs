using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using XDbAccess.Dapper;
using XDbAccess.Demo.Interfaces;
using XDbAccess.Demo.Models;
using System.Linq;

namespace XDbAccess.Demo.Repositories
{
    public class OrderPostgreSQLRepository : BaseRepository<DapperTest2DbContext>, IOrderRepository
    {
        public OrderPostgreSQLRepository(IDbHelper<DapperTest2DbContext> dbHelper) : base(dbHelper) { }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            IEnumerable<Order> data;
            string sql = "select * from \"Order\"";
            data = await DbHelper.QueryAsync<Order>(sql);
            return data.ToList();
        }

        public async Task InsertOrderAsync(Order order)
        {
            order.Id = Convert.ToInt32(await DbHelper.InsertAsync<Order>(order));
        }

        public async Task InsertOrderProductRefAsync(int orderId, int productId)
        {
            string sql = $@"
                insert into ""OrderProductRef""(""OrderId"",""ProductId"") values(@OrderId,@ProductId) RETURNING CAST(""Id"" AS BIGINT);
            ";
            await DbHelper.ExecuteScalarAsync(sql, new { OrderId = orderId, ProductId = productId });
        }
    }
}
