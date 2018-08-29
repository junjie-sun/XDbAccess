using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.Demo.Interfaces;
using XDbAccess.Demo.Models;
using XDbAccess.Dapper;

namespace XDbAccess.Demo.Repositories
{
    public class OrderRepository : BaseRepository<DapperTest2DbContext>, IOrderRepository
    {
        public OrderRepository(IDbHelper<DapperTest2DbContext> dbHelper) : base(dbHelper) { }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            IEnumerable<Order> data;
            string sql = "select * from [Order]";
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
                insert into [OrderProductRef]([OrderId],[ProductId]) values(@OrderId,@ProductId);
                SELECT @@IDENTITY;
            ";
            await DbHelper.ExecuteScalarAsync(sql, new { OrderId = orderId, ProductId = productId });
        }
    }
}
