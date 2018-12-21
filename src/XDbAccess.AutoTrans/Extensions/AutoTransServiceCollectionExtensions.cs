// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace XDbAccess.AutoTrans
{
    /// <summary>
    /// 事务服务扩展类
    /// </summary>
    public static class AutoTransServiceCollectionExtensions
    {
        /// <summary>
        /// 添加DbContext
        /// </summary>
        /// <typeparam name="DbContextType"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <param name="optionsAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddDbContext<DbContextType>(this IServiceCollection serviceCollection, Action<DbContextOptionsBuilder> optionsAction) where DbContextType : class, IDbContext
        {
            var builder = new DbContextOptionsBuilder();
            optionsAction?.Invoke(builder);
            serviceCollection.AddSingleton((serviceProvider) =>
            {
                var dbContext = (DbContextType)Activator.CreateInstance(typeof(DbContextType), builder.Options);
                return dbContext;
            });
            return serviceCollection;
        }
    }
}