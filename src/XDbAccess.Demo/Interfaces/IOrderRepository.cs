using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.Demo.Models;

namespace XDbAccess.Demo.Interfaces
{
    public interface IOrderRepository
    {
        Task InsertOrderAsync(Order order);

        Task<List<Order>> GetAllOrdersAsync();

        Task InsertOrderProductRefAsync(int orderId, int productId);
    }
}
