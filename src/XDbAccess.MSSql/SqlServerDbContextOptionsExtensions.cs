// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.AutoTrans;

namespace XDbAccess.MSSql
{
    /// <summary>
    /// MSSQL扩展类
    /// </summary>
    public static class SqlServerDbContextOptionsExtensions
    {
        /// <summary>
        /// 为DbContext配置数据库为MSSQL
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder UseSqlServer(this DbContextOptionsBuilder optionsBuilder, string connectionString)
        {
            optionsBuilder.Options.ConnectionString = connectionString;
            optionsBuilder.Options.DbFactory = new MSSqlDbFactory(connectionString);
            return optionsBuilder;
        }
    }
}
