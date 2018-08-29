using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.Demo.Interfaces;
using XDbAccess.Demo.Models;
using XDbAccess.Demo.Repositories;

namespace XDbAccess.Demo.Services
{
    public class OrderService : IOrderService
    {
        private IOrderRepository _OrderRepo;

        private DapperTest2DbContext _DbContext;

        public OrderService(IOrderRepository orderRepo, DapperTest2DbContext dbContext)
        {
            _OrderRepo = orderRepo;
            _DbContext = dbContext;
        }

        public async Task AddOrderAsync(Order order, List<int> productIdList)
        {
            using (var scope = _DbContext.TransScope())
            {
                await _OrderRepo.InsertOrderAsync(order);
                foreach (var productId in productIdList)
                {
                    await _OrderRepo.InsertOrderProductRefAsync(order.Id, productId);
                }
                scope.Commit();
            }
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _OrderRepo.GetAllOrdersAsync();
        }
    }
}
