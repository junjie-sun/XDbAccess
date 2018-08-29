using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.Demo.Models;

namespace XDbAccess.Demo.Interfaces
{
    public interface IOrderService
    {
        Task AddOrderAsync(Order order, List<int> productIdList);

        Task<List<Order>> GetAllOrdersAsync();
    }
}
