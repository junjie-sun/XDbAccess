// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using XDbAccess.AutoTrans;

namespace XDbAccess.PostgreSQL
{
    /// <summary>
    /// PostgreSQL扩展类
    /// </summary>
    public static class PostgreSQLDbContextOptionsExtensions
    {
        /// <summary>
        /// 为DbContext配置数据库为PostgreSQL
        /// </summary>
        /// <param name="optionsBuilder"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder UsePostgreSQL(this DbContextOptionsBuilder optionsBuilder, string connectionString)
        {
            optionsBuilder.Options.ConnectionString = connectionString;
            optionsBuilder.Options.DbFactory = new PostgreSQLDbFactory(connectionString);
            return optionsBuilder;
        }
    }
}
