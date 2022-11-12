// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using XDbAccess.AutoTrans;

namespace XDbAccess.Dapper
{
    /// <summary>
    /// DbHelper扩展类
    /// </summary>
    public static class DbHelperServiceCollectionExtensions
    {
        /// <summary>
        /// 添加MSSqlHepler
        /// </summary>
        /// <typeparam name="DbContextImpl"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddMSSqlDbHepler<DbContextImpl>(this IServiceCollection serviceCollection) where DbContextImpl : IDbContext
        {
            serviceCollection.AddSingleton<IDbHelper<DbContextImpl>, MSSqlDbHelper<DbContextImpl>>();
            return serviceCollection;
        }

        /// <summary>
        /// 添加MySqlHepler
        /// </summary>
        /// <typeparam name="DbContextImpl"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddMySqlDbHepler<DbContextImpl>(this IServiceCollection serviceCollection) where DbContextImpl : IDbContext
        {
            serviceCollection.AddSingleton<IDbHelper<DbContextImpl>, MySqlDbHelper<DbContextImpl>>();
            return serviceCollection;
        }

        /// <summary>
        /// 添加PostgreSQLHepler
        /// </summary>
        /// <typeparam name="DbContextImpl"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddPostgreSQLDbHepler<DbContextImpl>(this IServiceCollection serviceCollection) where DbContextImpl : IDbContext
        {
            serviceCollection.AddSingleton<IDbHelper<DbContextImpl>, PostgreSQLDbHelper<DbContextImpl>>();
            return serviceCollection;
        }
    }
}