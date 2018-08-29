using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.Demo.Interfaces;
using XDbAccess.Demo.Repositories;
using XDbAccess.Demo.Services;

namespace XDbAccess.Demo
{
    public static class Bootstraper
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IUserService, UserService>();
            serviceCollection.AddSingleton<IOrderService, OrderService>();
            serviceCollection.AddSingleton<IProductService, ProductService>();

            return serviceCollection;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IUserRepository, UserRepository>();
            serviceCollection.AddSingleton<IOrderRepository, OrderRepository>();
            serviceCollection.AddSingleton<IProductRepository, ProductRepository>();

            return serviceCollection;
        }

        public static IServiceCollection AddMySqlRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IUserRepository, UserMySqlRepository>();
            serviceCollection.AddSingleton<IOrderRepository, OrderMySqlRepository>();
            serviceCollection.AddSingleton<IProductRepository, ProductMySqlRepository>();

            return serviceCollection;
        }
    }
}
