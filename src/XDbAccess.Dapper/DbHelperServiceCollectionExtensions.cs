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
    public static class DbHelperServiceCollectionExtensions
    {
        public static IServiceCollection AddMSSqlDbHepler<DbContextImpl>(this IServiceCollection serviceCollection) where DbContextImpl : IDbContext
        {
            serviceCollection.AddSingleton<IDbHelper<DbContextImpl>, MSSqlDbHelper<DbContextImpl>>();
            return serviceCollection;
        }

        public static IServiceCollection AddMySqlDbHepler<DbContextImpl>(this IServiceCollection serviceCollection) where DbContextImpl : IDbContext
        {
            serviceCollection.AddSingleton<IDbHelper<DbContextImpl>, MySqlDbHelper<DbContextImpl>>();
            return serviceCollection;
        }
    }
}