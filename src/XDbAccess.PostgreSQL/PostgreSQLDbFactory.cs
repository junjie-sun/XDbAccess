// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using XDbAccess.AutoTrans;

namespace XDbAccess.PostgreSQL
{
    /// <summary>
    /// PostgreSQL工厂
    /// </summary>
    public class PostgreSQLDbFactory : IDbFactory
    {
        private string _ConnectionString;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString"></param>
        public PostgreSQLDbFactory(string connectionString)
        {
            _ConnectionString = connectionString;
        }

        /// <summary>
        /// 创建连接对象
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_ConnectionString);
        }
    }
}
